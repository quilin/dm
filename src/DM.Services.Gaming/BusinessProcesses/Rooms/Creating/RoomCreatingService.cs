using System.Threading;
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
internal class RoomCreatingService(
    IGameReadingService gameReadingService,
    IValidator<CreateRoom> validator,
    IIntentionManager intentionManager,
    IRoomFactory roomFactory,
    IUpdateBuilderFactory updateBuilderFactory,
    IRoomCreatingRepository repository,
    IUnreadCountersRepository unreadCountersRepository,
    IInvokedEventProducer producer) : IRoomCreatingService
{
    /// <inheritdoc />
    public async Task<Room> Create(CreateRoom createRoom, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(createRoom, cancellationToken);
        var game = await gameReadingService.GetGame(createRoom.GameId, cancellationToken);
        intentionManager.ThrowIfForbidden(GameIntention.Edit, game);

        var lastRoom = await repository.GetLastRoomInfo(createRoom.GameId, cancellationToken);

        var roomToCreate = lastRoom == null
            ? roomFactory.CreateFirst(createRoom)
            : roomFactory.CreateAfter(createRoom, lastRoom.Id, lastRoom.OrderNumber);
        var updateLastRoom = lastRoom == null
            ? null
            : updateBuilderFactory.Create<DbRoom>(lastRoom.Id).Field(r => r.NextRoomId, roomToCreate.RoomId);

        var room = await repository.Create(roomToCreate, updateLastRoom, cancellationToken);
        await unreadCountersRepository.Create(room.Id, game.Id, UnreadEntryType.Message, cancellationToken);
        await producer.Send(EventType.NewRoom, room.Id);

        return room;
    }
}