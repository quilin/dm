using System;
using System.Threading;
using System.Threading.Tasks;

namespace DM.Services.Gaming.BusinessProcesses.Pending.Deleting;

/// <summary>
/// Service for pending post deleting
/// </summary>
public interface IPendingPostDeletingService
{
    /// <summary>
    /// Delete existing pending
    /// </summary>
    /// <param name="pendingPostId">Pending post identifier</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task Delete(Guid pendingPostId, CancellationToken cancellationToken);
}