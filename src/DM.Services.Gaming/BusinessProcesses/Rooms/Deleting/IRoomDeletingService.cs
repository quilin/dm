using System;
using System.Threading;
using System.Threading.Tasks;

namespace DM.Services.Gaming.BusinessProcesses.Rooms.Deleting;

/// <summary>
/// Service for room deleting
/// </summary>
public interface IRoomDeletingService
{
    /// <summary>
    /// Delete existing room
    /// </summary>
    /// <param name="roomId">Room identifier</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task Delete(Guid roomId, CancellationToken cancellationToken);
}