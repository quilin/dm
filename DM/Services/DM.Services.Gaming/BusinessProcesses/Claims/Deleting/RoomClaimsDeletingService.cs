using System;
using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Common.Authorization;
using DM.Services.DataAccess.BusinessObjects.Games.Links;
using DM.Services.DataAccess.RelationalStorage;
using DM.Services.Gaming.Authorization;
using DM.Services.Gaming.BusinessProcesses.Claims.Reading;
using DM.Services.Gaming.BusinessProcesses.Rooms.Updating;

namespace DM.Services.Gaming.BusinessProcesses.Claims.Deleting
{
    /// <inheritdoc />
    public class RoomClaimsDeletingService : IRoomClaimsDeletingService
    {
        private readonly IRoomClaimsDeletingRepository repository;
        private readonly IRoomClaimsReadingRepository readingRepository;
        private readonly IRoomUpdatingRepository roomUpdatingRepository;
        private readonly IIntentionManager intentionManager;
        private readonly IUpdateBuilderFactory updateBuilderFactory;
        private readonly IIdentity identity;

        /// <inheritdoc />
        public RoomClaimsDeletingService(
            IRoomClaimsDeletingRepository repository,
            IRoomClaimsReadingRepository readingRepository,
            IRoomUpdatingRepository roomUpdatingRepository,
            IIntentionManager intentionManager,
            IUpdateBuilderFactory updateBuilderFactory,
            IIdentityProvider identityProvider)
        {
            this.repository = repository;
            this.readingRepository = readingRepository;
            this.roomUpdatingRepository = roomUpdatingRepository;
            this.intentionManager = intentionManager;
            this.updateBuilderFactory = updateBuilderFactory;
            identity = identityProvider.Current;
        }
        
        /// <inheritdoc />
        public async Task Delete(Guid claimId)
        {
            var oldClaim = await readingRepository.Get(claimId, identity.User.UserId);
            var room = await roomUpdatingRepository.GetRoom(oldClaim.RoomId, identity.User.UserId);
            await intentionManager.ThrowIfForbidden(GameIntention.AdministrateRooms, room.Game);
            
            var updateBuilder = updateBuilderFactory.Create<ParticipantRoomLink>(claimId, true);
            await repository.Delete(updateBuilder);
        }
    }
}