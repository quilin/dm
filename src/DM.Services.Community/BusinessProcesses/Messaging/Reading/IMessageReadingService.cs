using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DM.Services.Core.Dto;

namespace DM.Services.Community.BusinessProcesses.Messaging.Reading;

/// <summary>
/// Service for reading conversation messages
/// </summary>
public interface IMessageReadingService
{
    /// <summary>
    /// Get list of conversation messages
    /// </summary>
    /// <param name="conversationId">Conversation identifier</param>
    /// <param name="query">Paging query</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<(IEnumerable<Message> messages, PagingResult paging)> Get(Guid conversationId, PagingQuery query,
        CancellationToken cancellationToken);

    /// <summary>
    /// Get single message
    /// </summary>
    /// <param name="messageId">Message identifier</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Message> Get(Guid messageId, CancellationToken cancellationToken);
}