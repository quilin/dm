using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Common.BusinessProcesses.UnreadCounters;
using DM.Services.Common.Extensions;
using DM.Services.Core.Dto;
using DM.Services.Core.Exceptions;
using DM.Services.DataAccess.BusinessObjects.Common;

namespace DM.Services.Community.BusinessProcesses.Messaging.Reading;

/// <inheritdoc />
internal class ConversationReadingService(
    IConversationFactory factory,
    IConversationReadingRepository repository,
    IUnreadCountersRepository unreadCountersRepository,
    IIdentityProvider identityProvider) : IConversationReadingService
{
    /// <inheritdoc />
    public async Task<(IEnumerable<Conversation> conversations, PagingResult paging)> Get(
        PagingQuery query, CancellationToken cancellationToken)
    {
        var identity = identityProvider.Current;
        var currentUserId = identity.User.UserId;
        var totalCount = await repository.Count(currentUserId, cancellationToken);
        var pagingData = new PagingData(query, identity.Settings.Paging.MessagesPerPage, totalCount);
        var conversations = (await repository.Get(currentUserId, pagingData, cancellationToken)).ToArray();
        await unreadCountersRepository.FillEntityCounters(conversations, currentUserId,
            c => c.Id, c => c.UnreadMessagesCount, UnreadEntryType.Message, cancellationToken);

        return (conversations, pagingData.Result);
    }

    /// <inheritdoc />
    public async Task<Conversation> Get(Guid conversationId, CancellationToken cancellationToken)
    {
        var currentUserId = identityProvider.Current.User.UserId;
        var conversation = await repository.Get(conversationId, currentUserId, cancellationToken);
        if (conversation == null)
        {
            throw new HttpException(HttpStatusCode.Gone, "Conversation not found");
        }

        await unreadCountersRepository.FillEntityCounters(new[] {conversation}, currentUserId,
            c => c.Id, c => c.UnreadMessagesCount, UnreadEntryType.Message, cancellationToken);

        return conversation;
    }

    /// <inheritdoc />
    public async Task<Conversation> GetOrCreate(string login, CancellationToken cancellationToken)
    {
        var visaviId = await repository.FindUser(login, cancellationToken);
        if (!visaviId.HasValue)
        {
            throw new HttpException(HttpStatusCode.Gone, "User not found");
        }

        var currentUserId = identityProvider.Current.User.UserId;
        var existingConversation = await repository.FindVisaviConversation(currentUserId, visaviId.Value, cancellationToken);
        if (existingConversation != null)
        {
            await unreadCountersRepository.FillEntityCounters(new[] {existingConversation}, currentUserId,
                c => c.Id, c => c.UnreadMessagesCount, UnreadEntryType.Message, cancellationToken);
            return existingConversation;
        }

        var (conversation, conversationLinks) = factory.CreateVisavi(currentUserId, visaviId.Value);
        var result = await repository.Create(conversation, conversationLinks, cancellationToken);

        await unreadCountersRepository.Create(result.Id, UnreadEntryType.Message,
            new[] {currentUserId, visaviId.Value}.Distinct().ToArray(), cancellationToken);

        return result;
    }

    /// <inheritdoc />
    public async Task<int> GetTotalUnreadCount(CancellationToken cancellationToken)
    {
        var userId = identityProvider.Current.User.UserId;
        return (await unreadCountersRepository.SelectByParents(userId, UnreadEntryType.Message, [userId], cancellationToken))[userId];
    }

    /// <inheritdoc />
    public Task MarkAsRead(Guid conversationId, CancellationToken cancellationToken) =>
        unreadCountersRepository.Flush(identityProvider.Current.User.UserId,
            UnreadEntryType.Message, conversationId, cancellationToken);
}