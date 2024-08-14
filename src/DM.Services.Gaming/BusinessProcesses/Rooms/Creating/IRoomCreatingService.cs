using System.Threading.Tasks;
using DM.Services.Gaming.Dto.Input;
using DM.Services.Gaming.Dto.Output;

namespace DM.Services.Gaming.BusinessProcesses.Rooms.Creating;

/// <summary>
/// Creating service for rooms
/// </summary>
public interface IRoomCreatingService
{
    /// <summary>
    /// Create new room
    /// </summary>
    /// <param name="createRoom">DTO for room creating</param>
    /// <returns></returns>
    Task<Room> Create(CreateRoom createRoom);
}