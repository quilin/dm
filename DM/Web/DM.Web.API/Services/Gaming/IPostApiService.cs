using System;
using System.Threading.Tasks;
using DM.Services.Core.Dto;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Games;

namespace DM.Web.API.Services.Gaming;

/// <summary>
/// API service for game posts
/// </summary>
public interface IPostApiService
{
    /// <summary>
    /// Get room posts
    /// </summary>
    /// <param name="roomId">Room identifier</param>
    /// <param name="query">Search query</param>
    /// <returns></returns>
    Task<ListEnvelope<Post>> Get(Guid roomId, PagingQuery query);

    /// <summary>
    /// Get single post
    /// </summary>
    /// <param name="postId">Post identifier</param>
    /// <returns></returns>
    Task<Envelope<Post>> Get(Guid postId);

    /// <summary>
    /// Create new post
    /// </summary>
    /// <param name="roomId">Room identifier</param>
    /// <param name="post">Post model</param>
    /// <returns></returns>
    Task<Envelope<Post>> Create(Guid roomId, Post post);

    /// <summary>
    /// Update existing post
    /// </summary>
    /// <param name="postId">Post identifier</param>
    /// <param name="post">Post model</param>
    /// <returns></returns>
    Task<Envelope<Post>> Update(Guid postId, Post post);

    /// <summary>
    /// Delete existing post
    /// </summary>
    /// <param name="postId">Post identifier</param>
    /// <returns></returns>
    Task Delete(Guid postId);

    /// <summary>
    /// Mark all room posts as read
    /// </summary>
    /// <returns></returns>
    Task MarkAsRead(Guid roomId);
}