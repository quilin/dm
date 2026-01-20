using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DM.Services.Gaming.BusinessProcesses.Rooms.Creating;
using DM.Services.Gaming.BusinessProcesses.Rooms.Deleting;
using DM.Services.Gaming.BusinessProcesses.Rooms.Reading;
using DM.Services.Gaming.BusinessProcesses.Rooms.Updating;
using DM.Services.Gaming.Dto.Input;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Games;

namespace DM.Web.API.Services.Gaming.Rooms;

/// <inheritdoc />
internal class RoomApiService(
    IRoomReadingService readingService,
    IRoomCreatingService creatingService,
    IRoomUpdatingService updatingService,
    IRoomDeletingService deletingService,
    IMapper mapper) : IRoomApiService
{
    /// <inheritdoc />
    public async Task<ListEnvelope<Room>> GetAll(Guid gameId, CancellationToken cancellationToken)
    {
        var rooms = await readingService.GetAll(gameId, cancellationToken);
        return new ListEnvelope<Room>(rooms.Select(mapper.Map<Room>));
    }

    /// <inheritdoc />
    public async Task<Envelope<Room>> Get(Guid roomId, CancellationToken cancellationToken)
    {
        var room = await readingService.Get(roomId, cancellationToken);
        return new Envelope<Room>(mapper.Map<Room>(room));
    }

    /// <inheritdoc />
    public async Task<Envelope<Room>> Create(Guid gameId, Room room, CancellationToken cancellationToken)
    {
        var createRoom = mapper.Map<CreateRoom>(room);
        createRoom.GameId = gameId;
        var createdRoom = await creatingService.Create(createRoom, cancellationToken);
        return new Envelope<Room>(mapper.Map<Room>(createdRoom));
    }

    /// <inheritdoc />
    public async Task<Envelope<Room>> Update(Guid roomId, Room room, CancellationToken cancellationToken)
    {
        var updateRoom = mapper.Map<UpdateRoom>(room);
        updateRoom.RoomId = roomId;
        var updatedRoom = await updatingService.Update(updateRoom, cancellationToken);
        return new Envelope<Room>(mapper.Map<Room>(updatedRoom));
    }

    /// <inheritdoc />
    public Task Delete(Guid roomId, CancellationToken cancellationToken) =>
        deletingService.Delete(roomId, cancellationToken);
}