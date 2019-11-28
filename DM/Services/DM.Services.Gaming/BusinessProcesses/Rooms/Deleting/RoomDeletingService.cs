using System;
using System.Threading.Tasks;
using DM.Services.Common.Authorization;
using DM.Services.Common.BusinessProcesses.UnreadCounters;
using DM.Services.DataAccess.BusinessObjects.Common;
using DM.Services.DataAccess.RelationalStorage;
using DM.Services.Gaming.Authorization;
using DM.Services.Gaming.BusinessProcesses.Rooms.Reading;
using DM.Services.Gaming.BusinessProcesses.Rooms.Updating;
using DbRoom = DM.Services.DataAccess.BusinessObjects.Games.Posts.Room;

namespace DM.Services.Gaming.BusinessProcesses.Rooms.Deleting
{
    /// <inheritdoc />
    public class RoomDeletingService : IRoomDeletingService
    {
        private readonly IRoomReadingService roomReadingService;
        private readonly IIntentionManager intentionManager;
        private readonly IUpdateBuilderFactory updateBuilderFactory;
        private readonly IRoomUpdatingRepository repository;
        private readonly IUnreadCountersRepository unreadCountersRepository;

        /// <inheritdoc />
        public RoomDeletingService(
            IRoomReadingService roomReadingService,
            IIntentionManager intentionManager,
            IUpdateBuilderFactory updateBuilderFactory,
            IRoomUpdatingRepository repository,
            IUnreadCountersRepository unreadCountersRepository)
        {
            this.roomReadingService = roomReadingService;
            this.intentionManager = intentionManager;
            this.updateBuilderFactory = updateBuilderFactory;
            this.repository = repository;
            this.unreadCountersRepository = unreadCountersRepository;
        }
        
        /// <inheritdoc />
        public async Task Delete(Guid roomId)
        {
            var room = await roomReadingService.Get(roomId);
            await intentionManager.ThrowIfForbidden(GameIntention.AdministrateRooms, room.Game);

            var updateRoom = updateBuilderFactory.Create<DbRoom>(roomId).Field(r => r.IsRemoved, true);
            var updateOldPreviousRoom = room.PreviousRoomId.HasValue
                ? updateBuilderFactory.Create<DbRoom>(room.PreviousRoomId.Value)
                    .Field(r => r.NextRoomId, room.NextRoomId)
                : null;
            var updateOldNextRoom = room.NextRoomId.HasValue
                ? updateBuilderFactory.Create<DbRoom>(room.NextRoomId.Value)
                    .Field(r => r.PreviousRoomId, room.PreviousRoomId)
                : null;

            await repository.Update(updateRoom, updateOldNextRoom, updateOldPreviousRoom);
            await unreadCountersRepository.Delete(roomId, UnreadEntryType.Message);
        }
    }
}