using System;
using System.Threading;
using System.Threading.Tasks;

namespace DM.Services.Gaming.BusinessProcesses.Posts.Deleting;

/// <summary>
/// Service for post deleting
/// </summary>
public interface IPostDeletingService
{
    /// <summary>
    /// Delete existing post
    /// </summary>
    /// <param name="postId">Post identifier</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task Delete(Guid postId, CancellationToken cancellationToken);
}