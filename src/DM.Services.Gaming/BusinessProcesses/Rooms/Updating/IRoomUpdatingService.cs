using System.Threading;
using System.Threading.Tasks;
using DM.Services.Gaming.Dto.Input;
using DM.Services.Gaming.Dto.Output;

namespace DM.Services.Gaming.BusinessProcesses.Rooms.Updating;

/// <summary>
/// Updating service for rooms
/// </summary>
public interface IRoomUpdatingService
{
    /// <summary>
    /// Update existing room
    /// </summary>
    /// <param name="updateRoom">DTO for room updating</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Room> Update(UpdateRoom updateRoom, CancellationToken cancellationToken);
}