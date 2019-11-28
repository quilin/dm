using System.Threading.Tasks;
using DM.Services.Common.Authorization;
using DM.Services.Common.BusinessProcesses.UnreadCounters;
using DM.Services.DataAccess.BusinessObjects.Common;
using DM.Services.DataAccess.RelationalStorage;
using DM.Services.Gaming.Authorization;
using DM.Services.Gaming.BusinessProcesses.Games.Reading;
using DM.Services.Gaming.Dto.Input;
using DM.Services.Gaming.Dto.Output;
using FluentValidation;
using DbRoom = DM.Services.DataAccess.BusinessObjects.Games.Posts.Room;

namespace DM.Services.Gaming.BusinessProcesses.Rooms.Creating
{
    /// <inheritdoc />
    public class RoomCreatingService : IRoomCreatingService
    {
        private readonly IGameReadingService gameReadingService;
        private readonly IValidator<CreateRoom> validator;
        private readonly IIntentionManager intentionManager;
        private readonly IRoomFactory roomFactory;
        private readonly IUpdateBuilderFactory updateBuilderFactory;
        private readonly IRoomCreatingRepository repository;
        private readonly IUnreadCountersRepository unreadCountersRepository;

        /// <inheritdoc />
        public RoomCreatingService(
            IGameReadingService gameReadingService,
            IValidator<CreateRoom> validator,
            IIntentionManager intentionManager,
            IRoomFactory roomFactory,
            IUpdateBuilderFactory updateBuilderFactory,
            IRoomCreatingRepository repository,
            IUnreadCountersRepository unreadCountersRepository)
        {
            this.gameReadingService = gameReadingService;
            this.validator = validator;
            this.intentionManager = intentionManager;
            this.roomFactory = roomFactory;
            this.updateBuilderFactory = updateBuilderFactory;
            this.repository = repository;
            this.unreadCountersRepository = unreadCountersRepository;
        }

        /// <inheritdoc />
        public async Task<Room> Create(CreateRoom createRoom)
        {
            await validator.ValidateAndThrowAsync(createRoom);
            var game = await gameReadingService.GetGame(createRoom.GameId);
            await intentionManager.ThrowIfForbidden(GameIntention.AdministrateRooms, game);

            var lastRoom = await repository.GetLastRoomInfo(createRoom.GameId);

            var roomToCreate = lastRoom == null
                ? roomFactory.CreateFirst(createRoom)
                : roomFactory.CreateAfter(createRoom, lastRoom.RoomId, lastRoom.OrderNumber);
            var updateLastRoom = lastRoom == null
                ? null
                : updateBuilderFactory.Create<DbRoom>(lastRoom.RoomId).Field(r => r.NextRoomId, roomToCreate.RoomId);

            var room = await repository.Create(roomToCreate, updateLastRoom);
            await unreadCountersRepository.Create(room.Id, game.Id, UnreadEntryType.Message);
            return room;
        }
    }
}