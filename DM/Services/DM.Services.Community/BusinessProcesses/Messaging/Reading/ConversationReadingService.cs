using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Common.BusinessProcesses.UnreadCounters;
using DM.Services.Common.Extensions;
using DM.Services.Core.Dto;
using DM.Services.Core.Exceptions;
using DM.Services.DataAccess.BusinessObjects.Common;

namespace DM.Services.Community.BusinessProcesses.Messaging.Reading;

/// <inheritdoc />
internal class ConversationReadingService : IConversationReadingService
{
    private readonly IConversationFactory factory;
    private readonly IConversationReadingRepository repository;
    private readonly IUnreadCountersRepository unreadCountersRepository;
    private readonly IIdentityProvider identityProvider;

    /// <inheritdoc />
    public ConversationReadingService(
        IConversationFactory factory,
        IConversationReadingRepository repository,
        IUnreadCountersRepository unreadCountersRepository,
        IIdentityProvider identityProvider)
    {
        this.factory = factory;
        this.repository = repository;
        this.unreadCountersRepository = unreadCountersRepository;
        this.identityProvider = identityProvider;
    }

    /// <inheritdoc />
    public async Task<(IEnumerable<Conversation> conversations, PagingResult paging)> Get(PagingQuery query)
    {
        var identity = identityProvider.Current;
        var currentUserId = identity.User.UserId;
        var totalCount = await repository.Count(currentUserId);
        var pagingData = new PagingData(query, identity.Settings.Paging.MessagesPerPage, totalCount);
        var conversations = (await repository.Get(currentUserId, pagingData)).ToArray();
        await unreadCountersRepository.FillEntityCounters(conversations, currentUserId,
            c => c.Id, c => c.UnreadMessagesCount);

        return (conversations, pagingData.Result);
    }

    /// <inheritdoc />
    public async Task<Conversation> Get(Guid conversationId)
    {
        var currentUserId = identityProvider.Current.User.UserId;
        var conversation = await repository.Get(conversationId, currentUserId);
        if (conversation == null)
        {
            throw new HttpException(HttpStatusCode.Gone, "Conversation not found");
        }

        await unreadCountersRepository.FillEntityCounters(new[] {conversation}, currentUserId,
            c => c.Id, c => c.UnreadMessagesCount);

        return conversation;
    }

    /// <inheritdoc />
    public async Task<Conversation> GetOrCreate(string login)
    {
        var visaviId = await repository.FindUser(login);
        if (!visaviId.HasValue)
        {
            throw new HttpException(HttpStatusCode.Gone, "User not found");
        }

        var currentUserId = identityProvider.Current.User.UserId;
        var existingConversation = await repository.FindVisaviConversation(currentUserId, visaviId.Value);
        if (existingConversation != null)
        {
            await unreadCountersRepository.FillEntityCounters(new[] {existingConversation}, currentUserId,
                c => c.Id, c => c.UnreadMessagesCount);
            return existingConversation;
        }

        var (conversation, conversationLinks) = factory.CreateVisavi(currentUserId, visaviId.Value);
        var result = await repository.Create(conversation, conversationLinks);

        await unreadCountersRepository.Create(result.Id, UnreadEntryType.Message,
            new[] {currentUserId, visaviId.Value}.Distinct());

        return result;
    }

    /// <inheritdoc />
    public async Task<int> GetTotalUnreadCount()
    {
        var userId = identityProvider.Current.User.UserId;
        return (await unreadCountersRepository.SelectByParents(userId, UnreadEntryType.Message, userId))[userId];
    }

    /// <inheritdoc />
    public Task MarkAsRead(Guid conversationId) =>
        unreadCountersRepository.Flush(identityProvider.Current.User.UserId,
            UnreadEntryType.Message, conversationId);
}