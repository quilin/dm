using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DM.Services.Core.Dto;
using DM.Services.Gaming.Dto.Output;

namespace DM.Services.Gaming.BusinessProcesses.Posts.Reading;

/// <summary>
/// Storage for reading game posts
/// </summary>
internal interface IPostReadingRepository
{
    /// <summary>
    /// Count posts in a room
    /// </summary>
    /// <param name="roomId">Room identifier</param>
    /// <param name="userId">User identifier</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<int> Count(Guid roomId, Guid userId, CancellationToken cancellationToken);

    /// <summary>
    /// Get room posts
    /// </summary>
    /// <param name="roomId">Room identifier</param>
    /// <param name="paging">Paging data</param>
    /// <param name="userId">User identifier</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IEnumerable<Post>> Get(Guid roomId, PagingData paging, Guid userId, CancellationToken cancellationToken);

    /// <summary>
    /// Get single room post
    /// </summary>
    /// <param name="postId">Post identifier</param>
    /// <param name="userId">User identifier</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Post> Get(Guid postId, Guid userId, CancellationToken cancellationToken);
}