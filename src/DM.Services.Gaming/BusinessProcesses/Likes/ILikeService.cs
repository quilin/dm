using System;
using System.Threading;
using System.Threading.Tasks;
using DM.Services.Core.Dto;

namespace DM.Services.Gaming.BusinessProcesses.Likes;

/// <summary>
/// Service for game likes
/// </summary>
public interface ILikeService
{
    /// <summary>
    /// Create new like from current user to selected comment
    /// </summary>
    /// <param name="commentId">Comment identifier</param>
    /// <param name="cancellationToken"></param>
    /// <returns>User who liked the comment</returns>
    Task<GeneralUser> LikeComment(Guid commentId, CancellationToken cancellationToken);

    /// <summary>
    /// Remove existing like from current user to selected comment
    /// </summary>
    /// <param name="commentId">Comment identifier</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task DislikeComment(Guid commentId, CancellationToken cancellationToken);
}