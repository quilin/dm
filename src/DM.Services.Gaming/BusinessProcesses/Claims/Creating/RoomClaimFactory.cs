using System;
using DM.Services.Core.Implementation;
using DM.Services.DataAccess.BusinessObjects.Games.Links;
using DM.Services.Gaming.Dto.Input;

namespace DM.Services.Gaming.BusinessProcesses.Claims.Creating;

/// <inheritdoc />
internal class RoomClaimFactory : IRoomClaimFactory
{
    private readonly IGuidFactory guidFactory;

    /// <inheritdoc />
    public RoomClaimFactory(
        IGuidFactory guidFactory)
    {
        this.guidFactory = guidFactory;
    }
        
    /// <inheritdoc />
    public RoomClaim Create(CreateRoomClaim roomClaim, Guid participantId)
    {
        return new RoomClaim
        {
            RoomClaimId = guidFactory.Create(),
            Policy = roomClaim.Policy,
            RoomId = roomClaim.RoomId,
            ParticipantId = participantId
        };
    }
}