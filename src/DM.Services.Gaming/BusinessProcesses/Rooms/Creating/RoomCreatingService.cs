using System.Threading.Tasks;
using DM.Services.Common.Authorization;
using DM.Services.Common.BusinessProcesses.UnreadCounters;
using DM.Services.Core.Dto.Enums;
using DM.Services.DataAccess.BusinessObjects.Common;
using DM.Services.DataAccess.RelationalStorage;
using DM.Services.Gaming.Authorization;
using DM.Services.Gaming.BusinessProcesses.Games.Reading;
using DM.Services.Gaming.Dto.Input;
using DM.Services.Gaming.Dto.Output;
using DM.Services.MessageQueuing.GeneralBus;
using FluentValidation;
using DbRoom = DM.Services.DataAccess.BusinessObjects.Games.Posts.Room;

namespace DM.Services.Gaming.BusinessProcesses.Rooms.Creating;

/// <inheritdoc />
internal class RoomCreatingService : IRoomCreatingService
{
    private readonly IGameReadingService gameReadingService;
    private readonly IValidator<CreateRoom> validator;
    private readonly IIntentionManager intentionManager;
    private readonly IRoomFactory roomFactory;
    private readonly IUpdateBuilderFactory updateBuilderFactory;
    private readonly IRoomCreatingRepository repository;
    private readonly IUnreadCountersRepository unreadCountersRepository;
    private readonly IInvokedEventProducer producer;

    /// <inheritdoc />
    public RoomCreatingService(
        IGameReadingService gameReadingService,
        IValidator<CreateRoom> validator,
        IIntentionManager intentionManager,
        IRoomFactory roomFactory,
        IUpdateBuilderFactory updateBuilderFactory,
        IRoomCreatingRepository repository,
        IUnreadCountersRepository unreadCountersRepository,
        IInvokedEventProducer producer)
    {
        this.gameReadingService = gameReadingService;
        this.validator = validator;
        this.intentionManager = intentionManager;
        this.roomFactory = roomFactory;
        this.updateBuilderFactory = updateBuilderFactory;
        this.repository = repository;
        this.unreadCountersRepository = unreadCountersRepository;
        this.producer = producer;
    }

    /// <inheritdoc />
    public async Task<Room> Create(CreateRoom createRoom)
    {
        await validator.ValidateAndThrowAsync(createRoom);
        var game = await gameReadingService.GetGame(createRoom.GameId);
        intentionManager.ThrowIfForbidden(GameIntention.Edit, game);

        var lastRoom = await repository.GetLastRoomInfo(createRoom.GameId);

        var roomToCreate = lastRoom == null
            ? roomFactory.CreateFirst(createRoom)
            : roomFactory.CreateAfter(createRoom, lastRoom.Id, lastRoom.OrderNumber);
        var updateLastRoom = lastRoom == null
            ? null
            : updateBuilderFactory.Create<DbRoom>(lastRoom.Id).Field(r => r.NextRoomId, roomToCreate.RoomId);

        var room = await repository.Create(roomToCreate, updateLastRoom);
        await unreadCountersRepository.Create(room.Id, game.Id, UnreadEntryType.Message);
        await producer.Send(EventType.NewRoom, room.Id);

        return room;
    }
}