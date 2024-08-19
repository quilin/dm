using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DM.Services.DataAccess;
using DM.Services.Gaming.BusinessProcesses.Shared;
using DM.Services.Gaming.Dto.Output;
using Microsoft.EntityFrameworkCore;

namespace DM.Services.Gaming.BusinessProcesses.Rooms.Reading;

/// <inheritdoc />
internal class RoomReadingRepository(
    DmDbContext dbContext,
    IMapper mapper) : IRoomReadingRepository
{
    /// <inheritdoc />
    public async Task<IEnumerable<Room>> GetAllAvailable(Guid gameId, Guid userId, CancellationToken cancellationToken) =>
        await dbContext.Rooms
            .Where(r => r.GameId == gameId)
            .Where(AccessibilityFilters.RoomAvailable(userId))
            .OrderBy(r => r.OrderNumber)
            .ProjectTo<Room>(mapper.ConfigurationProvider)
            .ToArrayAsync(cancellationToken);

    /// <inheritdoc />
    public Task<Room> GetAvailable(Guid roomId, Guid userId, CancellationToken cancellationToken) =>
        dbContext.Rooms
            .Where(r => r.RoomId == roomId)
            .Where(AccessibilityFilters.RoomAvailable(userId))
            .ProjectTo<Room>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);
}