using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Services.Core.Dto;
using DM.Services.Gaming.Dto.Output;

namespace DM.Services.Gaming.BusinessProcesses.Posts.Reading;

/// <summary>
/// Service for reading posts
/// </summary>
public interface IPostReadingService
{
    /// <summary>
    /// Get list of posts in the room
    /// </summary>
    /// <param name="roomId">Room identifier</param>
    /// <param name="query">Paging query</param>
    /// <returns></returns>
    Task<(IEnumerable<Post> posts, PagingResult paging)> Get(Guid roomId, PagingQuery query);

    /// <summary>
    /// Get single existing post
    /// </summary>
    /// <param name="postId">Post identifier</param>
    /// <returns></returns>
    Task<Post> Get(Guid postId);

    /// <summary>
    /// Mark all posts in a room as read
    /// </summary>
    /// <param name="roomId">Room identifier</param>
    /// <returns></returns>
    Task MarkAsRead(Guid roomId);
}