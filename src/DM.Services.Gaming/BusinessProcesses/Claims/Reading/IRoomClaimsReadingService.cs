using System;
using System.Collections.Generic;
using System.Threading;
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
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IEnumerable<RoomClaim>> GetGameClaims(Guid gameId, CancellationToken cancellationToken);

    /// <summary>
    /// Get all room claims
    /// </summary>
    /// <param name="roomId">Room identifier</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IEnumerable<RoomClaim>> GetRoomClaims(Guid roomId, CancellationToken cancellationToken);

    /// <summary>
    /// Get existing claim
    /// </summary>
    /// <param name="claimId">Claim identifier</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<RoomClaim> GetClaim(Guid claimId, CancellationToken cancellationToken);
}