using System;
using System.Threading.Tasks;
using DM.Services.Core.Dto;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Shared;

namespace DM.Web.API.Services.Gaming;

/// <summary>
/// API service for game commentaries
/// </summary>
public interface ICommentApiService
{
    /// <summary>
    /// Get game commentaries
    /// </summary>
    /// <param name="gameId">Game identifier</param>
    /// <param name="query">Paging query</param>
    /// <returns>Envelope of commentaries list</returns>
    Task<ListEnvelope<Comment>> Get(Guid gameId, PagingQuery query);

    /// <summary>
    /// Create new comment
    /// </summary>
    /// <param name="gameId">Game identifier</param>
    /// <param name="comment">Comment model</param>
    /// <returns>Envelope of created comment</returns>
    Task<Envelope<Comment>> Create(Guid gameId, Comment comment);

    /// <summary>
    /// Get comment by identifier
    /// </summary>
    /// <param name="commentId">Comment identifier</param>
    /// <returns></returns>
    Task<Envelope<Comment>> Get(Guid commentId);

    /// <summary>
    /// Update comment by API DTO model
    /// </summary>
    /// <param name="commentId">Comment identifier</param>
    /// <param name="comment">Comment DTO model</param>
    /// <returns></returns>
    Task<Envelope<Comment>> Update(Guid commentId, Comment comment);

    /// <summary>
    /// Delete comment
    /// </summary>
    /// <param name="commentId">Comment identifier</param>
    /// <returns></returns>
    Task Delete(Guid commentId);

    /// <summary>
    /// Mark all game comments as read
    /// </summary>
    /// <param name="gameId"></param>
    /// <returns></returns>
    Task MarkAsRead(Guid gameId);
}