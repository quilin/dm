using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DM.Services.DataAccess;
using DM.Services.DataAccess.RelationalStorage;
using DM.Services.Gaming.BusinessProcesses.Shared;
using DM.Services.Gaming.Dto.Internal;
using DM.Services.Gaming.Dto.Output;
using Microsoft.EntityFrameworkCore;
using DbRoom = DM.Services.DataAccess.BusinessObjects.Games.Posts.Room;

namespace DM.Services.Gaming.BusinessProcesses.Rooms.Updating;

/// <inheritdoc />
internal class RoomUpdatingRepository(
    DmDbContext dbContext,
    IMapper mapper) : IRoomUpdatingRepository
{
    /// <inheritdoc />
    public Task<RoomToUpdate> GetRoom(Guid roomId, Guid userId, CancellationToken cancellationToken) =>
        dbContext.Rooms
            .Where(r => r.RoomId == roomId)
            .Where(AccessibilityFilters.RoomAvailable(userId))
            .ProjectTo<RoomToUpdate>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

    /// <inheritdoc />
    public Task<RoomNeighbours> GetNeighbours(Guid roomId, CancellationToken cancellationToken) =>
        dbContext.Rooms
            .Where(r => r.RoomId == roomId)
            .ProjectTo<RoomNeighbours>(mapper.ConfigurationProvider)
            .FirstAsync(cancellationToken);

    /// <inheritdoc />
    public async Task<Room> Update(
        IUpdateBuilder<DbRoom> updateRoom,
        IUpdateBuilder<DbRoom> updateOldPreviousRoom,
        IUpdateBuilder<DbRoom> updateOldNextRoom,
        IUpdateBuilder<DbRoom> updateNewPreviousRoom,
        IUpdateBuilder<DbRoom> updateNewNextRoom,
        CancellationToken cancellationToken)
    {
        var roomId = updateRoom.AttachTo(dbContext);
        updateOldPreviousRoom?.AttachTo(dbContext);
        updateOldNextRoom?.AttachTo(dbContext);
        updateNewPreviousRoom?.AttachTo(dbContext);
        updateNewNextRoom?.AttachTo(dbContext);
        await dbContext.SaveChangesAsync(cancellationToken);

        return await dbContext.Rooms
            .Where(r => r.RoomId == roomId)
            .ProjectTo<Room>(mapper.ConfigurationProvider)
            .FirstAsync(cancellationToken);
    }


    /// <inheritdoc />
    public Task<RoomOrderInfo> GetFirstRoomInfo(Guid gameId, CancellationToken cancellationToken) =>
        dbContext.Rooms
            .Where(r => !r.IsRemoved && r.GameId == gameId)
            .OrderBy(r => r.OrderNumber)
            .ProjectTo<RoomOrderInfo>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);
}