using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DM.Services.DataAccess;
using DM.Services.DataAccess.RelationalStorage;
using DM.Services.Gaming.Dto.Internal;
using DM.Services.Gaming.Dto.Output;
using Microsoft.EntityFrameworkCore;
using DbRoom = DM.Services.DataAccess.BusinessObjects.Games.Posts.Room;

namespace DM.Services.Gaming.BusinessProcesses.Rooms.Creating;

/// <inheritdoc />
internal class RoomCreatingRepository(
    DmDbContext dbContext,
    IMapper mapper) : IRoomCreatingRepository
{
    /// <inheritdoc />
    public async Task<Room> Create(DbRoom room, IUpdateBuilder<DbRoom> updateLastRoom,
        CancellationToken cancellationToken)
    {
        dbContext.Rooms.Add(room);
        updateLastRoom?.AttachTo(dbContext);
        await dbContext.SaveChangesAsync(cancellationToken);

        return await dbContext.Rooms
            .Where(r => r.RoomId == room.RoomId)
            .ProjectTo<Room>(mapper.ConfigurationProvider)
            .FirstAsync(cancellationToken);
    }

    /// <inheritdoc />
    public Task<RoomOrderInfo> GetLastRoomInfo(Guid gameId, CancellationToken cancellationToken) =>
        dbContext.Rooms
            .Where(r => !r.IsRemoved && r.GameId == gameId)
            .OrderByDescending(r => r.OrderNumber)
            .ProjectTo<RoomOrderInfo>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);
}