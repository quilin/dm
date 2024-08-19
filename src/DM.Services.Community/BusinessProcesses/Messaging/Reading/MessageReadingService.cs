using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Core.Dto;
using DM.Services.Core.Exceptions;

namespace DM.Services.Community.BusinessProcesses.Messaging.Reading;

/// <inheritdoc />
internal class MessageReadingService(
    IConversationReadingService conversationReadingService,
    IMessageReadingRepository repository,
    IIdentityProvider identityProvider) : IMessageReadingService
{
    /// <inheritdoc />
    public async Task<(IEnumerable<Message> messages, PagingResult paging)> Get(
        Guid conversationId, PagingQuery query, CancellationToken cancellationToken)
    {
        await conversationReadingService.Get(conversationId, cancellationToken);
        var totalCount = await repository.Count(conversationId, cancellationToken);
        var pagingData = new PagingData(query,
            identityProvider.Current.Settings.Paging.MessagesPerPage, totalCount);
        var messages = await repository.Get(conversationId, pagingData, cancellationToken);
        return (messages, pagingData.Result);
    }

    /// <inheritdoc />
    public async Task<Message> Get(Guid messageId, CancellationToken cancellationToken)
    {
        var message = await repository.Get(messageId, identityProvider.Current.User.UserId, cancellationToken);
        if (message == null)
        {
            throw new HttpException(HttpStatusCode.Gone, "Message not found");
        }

        return message;
    }
}