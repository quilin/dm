using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Core.Dto;
using DM.Services.Core.Exceptions;

namespace DM.Services.Community.BusinessProcesses.Chat.Reading;

/// <inheritdoc />
internal class ChatReadingService(
    IChatReadingRepository repository,
    IIdentityProvider identityProvider) : IChatReadingService
{
    /// <inheritdoc />
    public async Task<(IEnumerable<ChatMessage> messages, PagingResult paging)> GetMessages(
        PagingQuery pagingQuery, CancellationToken cancellationToken)
    {
        var totalCount = await repository.Count(cancellationToken);
        var pageSize = identityProvider.Current.Settings.Paging.EntitiesPerPage;
        var pagingData = new PagingData(pagingQuery, pageSize, totalCount);
        var chatMessages = await repository.Get(pagingData, cancellationToken);

        return (chatMessages, pagingData.Result);
    }

    /// <inheritdoc />
    public Task<IEnumerable<ChatMessage>> GetNewMessages(DateTimeOffset since, CancellationToken cancellationToken) =>
        repository.Get(since, cancellationToken);

    /// <inheritdoc />
    public async Task<ChatMessage> GetMessage(Guid id, CancellationToken cancellationToken)
    {
        var chatMessage = await repository.Get(id, cancellationToken);
        if (chatMessage == null)
        {
            throw new HttpException(HttpStatusCode.Gone, "Message not found");
        }

        return chatMessage;
    }
}