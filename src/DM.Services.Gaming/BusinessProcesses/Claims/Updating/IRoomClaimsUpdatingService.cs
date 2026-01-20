using System.Threading;
using System.Threading.Tasks;
using DM.Services.Gaming.Dto.Input;
using DM.Services.Gaming.Dto.Output;

namespace DM.Services.Gaming.BusinessProcesses.Claims.Updating;

/// <summary>
/// Service for room claims updating
/// </summary>
public interface IRoomClaimsUpdatingService
{
    /// <summary>
    /// Update existing room claim
    /// </summary>
    /// <param name="updateRoomClaim">DTO for update</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<RoomClaim> Update(UpdateRoomClaim updateRoomClaim, CancellationToken cancellationToken);
}