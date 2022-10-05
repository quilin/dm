using System;
using DM.Services.DataAccess.BusinessObjects.Games.Links;
using DM.Services.Gaming.Dto.Input;

namespace DM.Services.Gaming.BusinessProcesses.Claims.Creating;

/// <summary>
/// Factory for room claim DAL model
/// </summary>
internal interface IRoomClaimFactory
{
    /// <summary>
    /// Create DAL model
    /// </summary>
    /// <param name="roomClaim">DTO model</param>
    /// <param name="participantId">Participant identifier</param>
    /// <returns></returns>
    RoomClaim Create(CreateRoomClaim roomClaim, Guid participantId);
}