using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Services.Core.Dto;

namespace DM.Services.Community.BusinessProcesses.Messaging.Reading;

/// <summary>
/// Service for reading user conversations
/// </summary>
public interface IConversationReadingService
{
    /// <summary>
    /// Get list of user conversations
    /// </summary>
    /// <param name="query">Paging query</param>
    /// <returns></returns>
    Task<(IEnumerable<Conversation> conversations, PagingResult paging)> Get(PagingQuery query);

    /// <summary>
    /// Get single conversation
    /// </summary>
    /// <param name="conversationId">Conversation identifier</param>
    /// <returns></returns>
    Task<Conversation> Get(Guid conversationId);

    /// <summary>
    /// Find user visavi conversation
    /// </summary>
    /// <param name="login">User login</param>
    /// <returns></returns>
    Task<Conversation> GetOrCreate(string login);

    /// <summary>
    /// Count all unread conversations
    /// </summary>
    /// <returns></returns>
    Task<int> GetTotalUnreadCount();

    /// <summary>
    /// Mark all conversation messages as read
    /// </summary>
    /// <param name="conversationId">Conversation identifier</param>
    /// <returns></returns>
    Task MarkAsRead(Guid conversationId);
}