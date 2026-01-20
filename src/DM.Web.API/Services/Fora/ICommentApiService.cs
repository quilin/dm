using System;
using System.Threading;
using System.Threading.Tasks;
using DM.Services.Core.Dto;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Shared;

namespace DM.Web.API.Services.Fora;

/// <summary>
/// API service for forum commentaries
/// </summary>
public interface ICommentApiService
{
    /// <summary>
    /// Get topics commentaries
    /// </summary>
    /// <param name="topicId">Topic identifier</param>
    /// <param name="query">Paging query</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Envelope of commentaries list</returns>
    Task<ListEnvelope<Comment>> Get(Guid topicId, PagingQuery query, CancellationToken cancellationToken);

    /// <summary>
    /// Create new comment
    /// </summary>
    /// <param name="topicId">Topic identifier</param>
    /// <param name="comment">Comment model</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Envelope of created comment</returns>
    Task<Envelope<Comment>> Create(Guid topicId, Comment comment, CancellationToken cancellationToken);

    /// <summary>
    /// Get comment by identifier
    /// </summary>
    /// <param name="commentId">Comment identifier</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Envelope<Comment>> Get(Guid commentId, CancellationToken cancellationToken);

    /// <summary>
    /// Update comment by API DTO model
    /// </summary>
    /// <param name="commentId">Comment identifier</param>
    /// <param name="comment">Comment DTO model</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Envelope<Comment>> Update(Guid commentId, Comment comment, CancellationToken cancellationToken);

    /// <summary>
    /// Delete comment
    /// </summary>
    /// <param name="commentId">Comment identifier</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task Delete(Guid commentId, CancellationToken cancellationToken);

    /// <summary>
    /// Mark topic comments as read
    /// </summary>
    /// <param name="topicId">Topic identifier</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task MarkAsRead(Guid topicId, CancellationToken cancellationToken);

    /// <summary>
    /// Mark all forum comments as read
    /// </summary>
    /// <param name="forumId">Forum identifier</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task MarkAsRead(string forumId, CancellationToken cancellationToken);
}