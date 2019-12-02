using System;
using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Common.Authorization;
using DM.Services.Common.BusinessProcesses.UnreadCounters;
using DM.Services.DataAccess.BusinessObjects.Common;
using DM.Services.DataAccess.RelationalStorage;
using DM.Services.Gaming.Authorization;
using DM.Services.Gaming.BusinessProcesses.Rooms.Updating;
using DbRoom = DM.Services.DataAccess.BusinessObjects.Games.Posts.Room;

namespace DM.Services.Gaming.BusinessProcesses.Rooms.Deleting
{
    /// <inheritdoc />
    public class RoomDeletingService : IRoomDeletingService
    {
        private readonly IIntentionManager intentionManager;
        private readonly IUpdateBuilderFactory updateBuilderFactory;
        private readonly IRoomOrderPull roomOrderPull;
        private readonly IRoomUpdatingRepository repository;
        private readonly IUnreadCountersRepository unreadCountersRepository;
        private readonly IIdentity identity;

        /// <inheritdoc />
        public RoomDeletingService(
            IIntentionManager intentionManager,
            IUpdateBuilderFactory updateBuilderFactory,
            IRoomOrderPull roomOrderPull,
            IRoomUpdatingRepository repository,
            IUnreadCountersRepository unreadCountersRepository,
            IIdentityProvider identityProvider)
        {
            this.intentionManager = intentionManager;
            this.updateBuilderFactory = updateBuilderFactory;
            this.roomOrderPull = roomOrderPull;
            this.repository = repository;
            this.unreadCountersRepository = unreadCountersRepository;
            identity = identityProvider.Current;
        }
        
        /// <inheritdoc />
        public async Task Delete(Guid roomId)
        {
            var room = await repository.GetRoom(roomId, identity.User.UserId);
            await intentionManager.ThrowIfForbidden(GameIntention.AdministrateRooms, room.Game);

            var updateRoom = updateBuilderFactory.Create<DbRoom>(roomId).Field(r => r.IsRemoved, true);
            var (updateOldPreviousRoom, updateOldNextRoom) = roomOrderPull.GetPullChanges(room);
            await repository.Update(updateRoom, updateOldNextRoom, updateOldPreviousRoom);
            await unreadCountersRepository.Delete(roomId, UnreadEntryType.Message);
        }
    }
}