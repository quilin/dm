using System;
using System.Threading.Tasks;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Games;

namespace DM.Web.API.Services.Gaming.Rooms;

/// <summary>
/// API service for room claims
/// </summary>
public interface IRoomClaimApiService
{
    /// <summary>
    /// Create new room claim
    /// </summary>
    /// <param name="roomId">Room identifier</param>
    /// <param name="claim">Claim</param>
    /// <returns></returns>
    Task<Envelope<RoomClaim>> Create(Guid roomId, RoomClaim claim);

    /// <summary>
    /// Update existing room claim
    /// </summary>
    /// <param name="claimId">Claim identifier</param>
    /// <param name="claim">Claim</param>
    /// <returns></returns>
    Task<Envelope<RoomClaim>> Update(Guid claimId, RoomClaim claim);

    /// <summary>
    /// Delete existing room claim
    /// </summary>
    /// <param name="claimId"></param>
    /// <returns></returns>
    Task Delete(Guid claimId);
}