using System;
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
    /// <returns>Envelope of commentaries list</returns>
    Task<ListEnvelope<Comment>> Get(Guid topicId, PagingQuery query);

    /// <summary>
    /// Create new comment
    /// </summary>
    /// <param name="topicId">Topic identifier</param>
    /// <param name="comment">Comment model</param>
    /// <returns>Envelope of created comment</returns>
    Task<Envelope<Comment>> Create(Guid topicId, Comment comment);

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
    /// Mark topic comments as read
    /// </summary>
    /// <param name="topicId">Topic identifier</param>
    /// <returns></returns>
    Task MarkAsRead(Guid topicId);

    /// <summary>
    /// Mark all forum comments as read
    /// </summary>
    /// <param name="forumId">Forum identifier</param>
    /// <returns></returns>
    Task MarkAsRead(string forumId);
}