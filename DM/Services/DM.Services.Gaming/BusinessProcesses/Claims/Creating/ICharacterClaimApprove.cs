using System;
using System.Threading.Tasks;
using DM.Services.Gaming.Dto.Internal;

namespace DM.Services.Gaming.BusinessProcesses.Claims.Creating;

/// <summary>
/// Validate and decide character participant in room claim
/// </summary>
internal interface ICharacterClaimApprove
{
    /// <summary>
    /// Decide reader participant identifier
    /// </summary>
    /// <param name="characterId">Character identifier</param>
    /// <param name="room">Room to update</param>
    /// <returns></returns>
    Task<Guid> GetParticipantId(Guid characterId, RoomToUpdate room);
}