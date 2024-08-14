using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Core.Dto;
using DM.Services.Core.Exceptions;

namespace DM.Services.Community.BusinessProcesses.Messaging.Reading;

/// <inheritdoc />
internal class MessageReadingService : IMessageReadingService
{
    private readonly IConversationReadingService conversationReadingService;
    private readonly IMessageReadingRepository repository;
    private readonly IIdentityProvider identityProvider;

    /// <inheritdoc />
    public MessageReadingService(
        IConversationReadingService conversationReadingService,
        IMessageReadingRepository repository,
        IIdentityProvider identityProvider)
    {
        this.conversationReadingService = conversationReadingService;
        this.repository = repository;
        this.identityProvider = identityProvider;
    }

    /// <inheritdoc />
    public async Task<(IEnumerable<Message> messages, PagingResult paging)> Get(
        Guid conversationId, PagingQuery query)
    {
        await conversationReadingService.Get(conversationId);
        var totalCount = await repository.Count(conversationId);
        var pagingData = new PagingData(query,
            identityProvider.Current.Settings.Paging.MessagesPerPage, totalCount);
        var messages = await repository.Get(conversationId, pagingData);
        return (messages, pagingData.Result);
    }

    /// <inheritdoc />
    public async Task<Message> Get(Guid messageId)
    {
        var message = await repository.Get(messageId, identityProvider.Current.User.UserId);
        if (message == null)
        {
            throw new HttpException(HttpStatusCode.Gone, "Message not found");
        }

        return message;
    }
}