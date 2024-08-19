using System;
using System.Threading;
using System.Threading.Tasks;
using DM.Services.Core.Dto;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Messaging;

namespace DM.Web.API.Services.Community;

/// <summary>
/// API service for messaging
/// </summary>
public interface IMessagingApiService
{
    /// <summary>
    /// Get list of user conversations
    /// </summary>
    /// <param name="query">Paging query</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<ListEnvelope<Conversation>> GetConversations(PagingQuery query, CancellationToken cancellationToken);

    /// <summary>
    /// Get single conversation
    /// </summary>
    /// <param name="id">Conversation identifier</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Envelope<Conversation>> GetConversation(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Get visavi conversation with user
    /// </summary>
    /// <param name="login"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Envelope<Conversation>> GetConversation(string login, CancellationToken cancellationToken);

    /// <summary>
    /// Get list of conversation messages
    /// </summary>
    /// <param name="conversationId">Conversation identifier</param>
    /// <param name="query">Paging query</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<ListEnvelope<Message>> GetMessages(Guid conversationId, PagingQuery query,
        CancellationToken cancellationToken);

    /// <summary>
    /// Create new message
    /// </summary>
    /// <param name="conversationId">Conversation identifier</param>
    /// <param name="message">Message</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Envelope<Message>> CreateMessage(Guid conversationId, Message message,
        CancellationToken cancellationToken);

    /// <summary>
    /// Get single message
    /// </summary>
    /// <param name="messageId">Message identifier</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Envelope<Message>> GetMessage(Guid messageId, CancellationToken cancellationToken);

    /// <summary>
    /// Delete single message
    /// </summary>
    /// <param name="messageId">Message identifier</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task DeleteMessage(Guid messageId, CancellationToken cancellationToken);

    /// <summary>
    /// Mark all conversation messages as read
    /// </summary>
    /// <param name="conversationId">Conversation identifier</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task MarkAsRead(Guid conversationId, CancellationToken cancellationToken);
}