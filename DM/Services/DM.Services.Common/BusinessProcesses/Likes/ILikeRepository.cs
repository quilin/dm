using System;
using System.Threading.Tasks;
using DM.Services.DataAccess.BusinessObjects.Common;

namespace DM.Services.Common.BusinessProcesses.Likes;

/// <summary>
/// Storage for topic likes
/// </summary>
public interface ILikeRepository
{
    /// <summary>
    /// Store new like
    /// </summary>
    /// <param name="like">Like DAL model</param>
    /// <returns></returns>
    Task Add(Like like);

    /// <summary>
    /// Delete like from storage
    /// </summary>
    /// <param name="topicId">Topic identifier</param>
    /// <param name="userId">User identifier</param>
    /// <returns></returns>
    Task Delete(Guid topicId, Guid userId);
}