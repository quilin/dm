using System;
using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Common.Authorization;
using DM.Services.Core.Dto.Enums;
using DM.Services.DataAccess.BusinessObjects.Games.Links;
using DM.Services.DataAccess.RelationalStorage;
using DM.Services.Gaming.Authorization;
using DM.Services.Gaming.BusinessProcesses.Claims.Reading;
using DM.Services.Gaming.BusinessProcesses.Rooms.Updating;
using DM.Services.MessageQueuing.Publish;

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
        private readonly IInvokedEventPublisher publisher;
        private readonly IIdentity identity;

        /// <inheritdoc />
        public RoomClaimsDeletingService(
            IRoomClaimsDeletingRepository repository,
            IRoomClaimsReadingRepository readingRepository,
            IRoomUpdatingRepository roomUpdatingRepository,
            IIntentionManager intentionManager,
            IUpdateBuilderFactory updateBuilderFactory,
            IInvokedEventPublisher publisher,
            IIdentityProvider identityProvider)
        {
            this.repository = repository;
            this.readingRepository = readingRepository;
            this.roomUpdatingRepository = roomUpdatingRepository;
            this.intentionManager = intentionManager;
            this.updateBuilderFactory = updateBuilderFactory;
            this.publisher = publisher;
            identity = identityProvider.Current;
        }

        /// <inheritdoc />
        public async Task Delete(Guid claimId)
        {
            var oldClaim = await readingRepository.GetClaim(claimId, identity.User.UserId);
            var room = await roomUpdatingRepository.GetRoom(oldClaim.RoomId, identity.User.UserId);
            await intentionManager.ThrowIfForbidden(GameIntention.AdministrateRooms, room.Game);

            var updateBuilder = updateBuilderFactory.Create<RoomClaim>(claimId).Delete();
            await repository.Delete(updateBuilder);
            await publisher.Publish(EventType.ChangedRoom, room.Id);
        }
    }
}