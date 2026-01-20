using System.Threading;
using System.Threading.Tasks;
using DM.Services.Gaming.Dto.Input;
using DM.Services.Gaming.Dto.Output;

namespace DM.Services.Gaming.BusinessProcesses.Claims.Creating;

/// <summary>
/// Service for room claim creating
/// </summary>
public interface IRoomClaimsCreatingService
{
    /// <summary>
    /// Create new room claims
    /// </summary>
    /// <param name="createRoomClaim">DTO model</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<RoomClaim> Create(CreateRoomClaim createRoomClaim, CancellationToken cancellationToken);
}