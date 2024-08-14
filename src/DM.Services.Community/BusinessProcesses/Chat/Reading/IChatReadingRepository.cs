using System;
using System.Collections.Generic;
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
    /// <returns></returns>
    Task<int> Count();

    /// <summary>
    /// Get chat messages
    /// </summary>
    /// <param name="pagingData"></param>
    /// <returns></returns>
    Task<IEnumerable<ChatMessage>> Get(PagingData pagingData);

    /// <summary>
    /// Get new messages since given moment
    /// </summary>
    /// <param name="since"></param>
    /// <returns></returns>
    Task<IEnumerable<ChatMessage>> Get(DateTimeOffset since);

    /// <summary>
    /// Get single chat message
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<ChatMessage> Get(Guid id);
}