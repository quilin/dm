using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DM.Services.Core.Dto;

namespace DM.Services.Community.BusinessProcesses.Chat.Reading;

/// <summary>
/// Storage for reading chat messages
/// </summary>
internal interface IChatReadingRepository
{
    /// <summary>
    /// Count chat messages
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<int> Count(CancellationToken cancellationToken);

    /// <summary>
    /// Get chat messages
    /// </summary>
    /// <param name="pagingData"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IEnumerable<ChatMessage>> Get(PagingData pagingData, CancellationToken cancellationToken);

    /// <summary>
    /// Get new messages since given moment
    /// </summary>
    /// <param name="since"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IEnumerable<ChatMessage>> Get(DateTimeOffset since, CancellationToken cancellationToken);

    /// <summary>
    /// Get single chat message
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<ChatMessage> Get(Guid id, CancellationToken cancellationToken);
}