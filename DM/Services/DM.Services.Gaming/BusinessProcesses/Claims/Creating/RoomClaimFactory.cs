using System;
using DM.Services.Core.Implementation;
using DM.Services.DataAccess.BusinessObjects.Games.Links;
using DM.Services.Gaming.Dto.Input;

namespace DM.Services.Gaming.BusinessProcesses.Claims.Creating
{
    /// <inheritdoc />
    public class RoomClaimFactory : IRoomClaimFactory
    {
        private readonly IGuidFactory guidFactory;

        /// <inheritdoc />
        public RoomClaimFactory(
            IGuidFactory guidFactory)
        {
            this.guidFactory = guidFactory;
        }
        
        /// <inheritdoc />
        public ParticipantRoomLink Create(CreateRoomClaim roomClaim, Guid participantId)
        {
            return new ParticipantRoomLink
            {
                ParticipantRoomLinkId = guidFactory.Create(),
                Policy = roomClaim.AccessPolicy,
                RoomId = roomClaim.RoomId,
                ParticipantId = participantId
            };
        }
    }
}