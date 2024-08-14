using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Services.Gaming.Dto.Output;

namespace DM.Services.Gaming.BusinessProcesses.Claims.Reading;

/// <summary>
/// Service for reading room claims
/// </summary>
public interface IRoomClaimsReadingService
{
    /// <summary>
    /// Get all game claims
    /// </summary>
    /// <param name="gameId">Game identifier</param>
    /// <returns></returns>
    Task<IEnumerable<RoomClaim>> GetGameClaims(Guid gameId);
        
    /// <summary>
    /// Get all room claims
    /// </summary>
    /// <param name="roomId">Room identifier</param>
    /// <returns></returns>
    Task<IEnumerable<RoomClaim>> GetRoomClaims(Guid roomId);

    /// <summary>
    /// Get existing claim
    /// </summary>
    /// <param name="claimId">Claim identifier</param>
    /// <returns></returns>
    Task<RoomClaim> GetClaim(Guid claimId);
}