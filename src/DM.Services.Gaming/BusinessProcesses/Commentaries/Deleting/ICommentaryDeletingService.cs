using System;
using System.Threading;
using System.Threading.Tasks;

namespace DM.Services.Gaming.BusinessProcesses.Commentaries.Deleting;

/// <summary>
/// Service for deleting forum commentaries
/// </summary>
public interface ICommentaryDeletingService
{
    /// <summary>
    /// Delete commentary by identifier
    /// </summary>
    /// <param name="commentId">Commentary identifier</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task Delete(Guid commentId, CancellationToken cancellationToken);
}