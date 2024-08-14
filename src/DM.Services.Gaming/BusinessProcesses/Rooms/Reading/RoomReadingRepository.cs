using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DM.Services.DataAccess;
using DM.Services.Gaming.BusinessProcesses.Shared;
using DM.Services.Gaming.Dto.Output;
using Microsoft.EntityFrameworkCore;

namespace DM.Services.Gaming.BusinessProcesses.Rooms.Reading;

/// <inheritdoc />
internal class RoomReadingRepository : IRoomReadingRepository
{
    private readonly DmDbContext dbContext;
    private readonly IMapper mapper;

    /// <inheritdoc />
    public RoomReadingRepository(
        DmDbContext dbContext,
        IMapper mapper)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Room>> GetAllAvailable(Guid gameId, Guid userId)
    {
        return await dbContext.Rooms
            .Where(r => r.GameId == gameId)
            .Where(AccessibilityFilters.RoomAvailable(userId))
            .OrderBy(r => r.OrderNumber)
            .ProjectTo<Room>(mapper.ConfigurationProvider)
            .ToArrayAsync();
    }

    /// <inheritdoc />
    public Task<Room> GetAvailable(Guid roomId, Guid userId)
    {
        return dbContext.Rooms
            .Where(r => r.RoomId == roomId)
            .Where(AccessibilityFilters.RoomAvailable(userId))
            .ProjectTo<Room>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();
    }
}