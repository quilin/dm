using System;
using System.Threading.Tasks;

namespace DM.Services.Forum.BusinessProcesses.Commentaries.Deleting;

/// <summary>
/// Service for deleting forum commentaries
/// </summary>
public interface ICommentaryDeletingService
{
    /// <summary>
    /// Delete commentary by identifier
    /// </summary>
    /// <param name="commentId">Commentary identifier</param>
    /// <returns></returns>
    Task Delete(Guid commentId);
}