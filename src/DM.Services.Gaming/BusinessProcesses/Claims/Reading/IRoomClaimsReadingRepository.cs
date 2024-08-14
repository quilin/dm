using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Services.Gaming.Dto.Output;

namespace DM.Services.Gaming.BusinessProcesses.Claims.Reading;

/// <summary>
/// Storage for room claims reading
/// </summary>
internal interface IRoomClaimsReadingRepository
{
    /// <summary>
    /// Get all game claims
    /// </summary>
    /// <param name="gameId">Game identifier</param>
    /// <param name="userId">User identifier</param>
    /// <returns></returns>
    Task<IEnumerable<RoomClaim>> GetGameClaims(Guid gameId, Guid userId);

    /// <summary>
    /// Get all room claims
    /// </summary>
    /// <param name="roomId">Room identifier</param>
    /// <param name="userId">User identifier</param>
    /// <returns></returns>
    Task<IEnumerable<RoomClaim>> GetRoomClaims(Guid roomId, Guid userId);

    /// <summary>
    /// Get existing room claim
    /// </summary>
    /// <param name="claimId">Claim identifier</param>
    /// <param name="userId">User identifier</param>
    /// <returns></returns>
    Task<RoomClaim> GetClaim(Guid claimId, Guid userId);
}