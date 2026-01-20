using System;
using System.Threading;
using System.Threading.Tasks;
using DM.Services.Gaming.Dto.Internal;

namespace DM.Services.Gaming.BusinessProcesses.Claims.Creating;

/// <summary>
/// Validate and decide reader participant in room claim
/// </summary>
internal interface IReaderClaimApprove
{
    /// <summary>
    /// Decide reader participant identifier
    /// </summary>
    /// <param name="readerLogin">Reader login</param>
    /// <param name="room">Room to update</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Guid> GetParticipantId(string readerLogin, RoomToUpdate room, CancellationToken cancellationToken);
}