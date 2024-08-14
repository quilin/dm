using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Services.Core.Dto;

namespace DM.Services.Community.BusinessProcesses.Messaging.Reading;

/// <summary>
/// Storage for reading the messages
/// </summary>
internal interface IMessageReadingRepository
{
    /// <summary>
    /// Count messages in conversation
    /// </summary>
    /// <param name="conversationId">Conversation identifier</param>
    /// <returns></returns>
    Task<int> Count(Guid conversationId);

    /// <summary>
    /// Get messages
    /// </summary>
    /// <param name="conversationId">Conversation identifier</param>
    /// <param name="paging">Paging data</param>
    /// <returns></returns>
    Task<IEnumerable<Message>> Get(Guid conversationId, PagingData paging);

    /// <summary>
    /// Get single message
    /// </summary>
    /// <param name="messageId">Message identifier</param>
    /// <param name="userId">User identifier</param>
    /// <returns></returns>
    Task<Message> Get(Guid messageId, Guid userId);
}