using System;
using System.Linq;
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
internal class RoomUpdatingRepository : IRoomUpdatingRepository
{
    private readonly DmDbContext dbContext;
    private readonly IMapper mapper;

    /// <inheritdoc />
    public RoomUpdatingRepository(
        DmDbContext dbContext,
        IMapper mapper)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }

    /// <inheritdoc />
    public Task<RoomToUpdate> GetRoom(Guid roomId, Guid userId)
    {
        return dbContext.Rooms
            .Where(r => r.RoomId == roomId)
            .Where(AccessibilityFilters.RoomAvailable(userId))
            .ProjectTo<RoomToUpdate>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();
    }

    /// <inheritdoc />
    public Task<RoomNeighbours> GetNeighbours(Guid roomId)
    {
        return dbContext.Rooms
            .Where(r => r.RoomId == roomId)
            .ProjectTo<RoomNeighbours>(mapper.ConfigurationProvider)
            .FirstAsync();
    }

    /// <inheritdoc />
    public async Task<Room> Update(
        IUpdateBuilder<DbRoom> updateRoom,
        IUpdateBuilder<DbRoom> updateOldPreviousRoom,
        IUpdateBuilder<DbRoom> updateOldNextRoom,
        IUpdateBuilder<DbRoom> updateNewPreviousRoom,
        IUpdateBuilder<DbRoom> updateNewNextRoom)
    {
        var roomId = updateRoom.AttachTo(dbContext);
        updateOldPreviousRoom?.AttachTo(dbContext);
        updateOldNextRoom?.AttachTo(dbContext);
        updateNewPreviousRoom?.AttachTo(dbContext);
        updateNewNextRoom?.AttachTo(dbContext);
        await dbContext.SaveChangesAsync();

        return await dbContext.Rooms
            .Where(r => r.RoomId == roomId)
            .ProjectTo<Room>(mapper.ConfigurationProvider)
            .FirstAsync();
    }


    /// <inheritdoc />
    public Task<RoomOrderInfo> GetFirstRoomInfo(Guid gameId)
    {
        return dbContext.Rooms
            .Where(r => !r.IsRemoved && r.GameId == gameId)
            .OrderBy(r => r.OrderNumber)
            .ProjectTo<RoomOrderInfo>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();
    }
}