using System;
using System.Threading;
using System.Threading.Tasks;
using DM.Services.Core.Dto;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Messaging;

namespace DM.Web.API.Services.Community;

/// <summary>
/// API service for chat
/// </summary>
public interface IChatApiService
{
    /// <summary>
    /// Get list of chat messages
    /// </summary>
    /// <param name="query">Search query</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<ListEnvelope<ChatMessage>> GetMessages(PagingQuery query, CancellationToken cancellationToken);

    /// <summary>
    /// Create new chat message
    /// </summary>
    /// <param name="message">Message</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Envelope<ChatMessage>> CreateMessage(ChatMessage message, CancellationToken cancellationToken);

    /// <summary>
    /// Get single chat message
    /// </summary>
    /// <param name="id">Message identifier</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Envelope<ChatMessage>> GetMessage(Guid id, CancellationToken cancellationToken);
}