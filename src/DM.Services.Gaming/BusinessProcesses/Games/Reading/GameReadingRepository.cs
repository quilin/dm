using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DM.Services.Core.Dto;
using DM.Services.Core.Dto.Enums;
using DM.Services.DataAccess;
using DM.Services.Gaming.BusinessProcesses.Shared;
using DM.Services.Gaming.Dto.Input;
using DM.Services.Gaming.Dto.Output;
using Microsoft.EntityFrameworkCore;

namespace DM.Services.Gaming.BusinessProcesses.Games.Reading;

/// <inheritdoc />
internal class GameReadingRepository : IGameReadingRepository
{
    private readonly DmDbContext dbContext;
    private readonly IMapper mapper;

    /// <inheritdoc />
    public GameReadingRepository(
        DmDbContext dbContext,
        IMapper mapper)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }

    /// <inheritdoc />
    public Task<int> Count(GamesQuery query, Guid userId)
    {
        return dbContext.Games
            .Where(AccessibilityFilters.GameAvailable(userId))
            .Where(g => query.Statuses.Contains(g.Status))
            .Where(g => !query.TagId.HasValue || g.GameTags.Any(t => t.TagId == query.TagId.Value))
            .CountAsync();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Game>> GetGames(PagingData pagingData, GamesQuery query, Guid userId)
    {
        return await dbContext.Games
            .Where(AccessibilityFilters.GameAvailable(userId))
            .Where(g => query.Statuses.Contains(g.Status))
            .Where(g => !query.TagId.HasValue || g.GameTags.Any(t => t.TagId == query.TagId.Value))
            .OrderBy(g => g.ReleaseDate ?? g.CreateDate)
            .Skip(pagingData.Skip)
            .Take(pagingData.Take)
            .ProjectTo<Game>(mapper.ConfigurationProvider)
            .ToArrayAsync();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Game>> GetOwn(Guid userId)
    {
        return await dbContext.Games
            .Where(AccessibilityFilters.GameAvailable(userId))
            .Where(g => g.Characters.Any(c =>
                            !c.IsRemoved && c.Status == CharacterStatus.Active && c.UserId == userId) ||
                        g.Readers.Any(r => r.UserId == userId) ||
                        g.MasterId == userId || g.AssistantId == userId || g.NannyId == userId)
            .ProjectTo<Game>(mapper.ConfigurationProvider)
            .ToArrayAsync();
    }

    /// <inheritdoc />
    public async Task<IDictionary<Guid, IEnumerable<Guid>>> GetAvailableRoomIds(IEnumerable<Guid> gameIds, Guid userId)
    {
        var roomsInGames = await dbContext.Rooms
            .Where(AccessibilityFilters.RoomAvailable(userId))
            .Where(r => gameIds.Contains(r.GameId))
            .Select(r => new {r.RoomId, r.GameId})
            .ToArrayAsync();
        return roomsInGames
            .GroupBy(g => g.GameId)
            .ToDictionary(g => g.Key, g => g.Select(r => r.RoomId));
    }

    /// <inheritdoc />
    public async Task<IEnumerable<PendingPost>> GetPendingPosts(IEnumerable<Guid> gameIds, Guid userId)
    {
        return await dbContext.Rooms
            .Where(AccessibilityFilters.RoomAvailable(userId))
            .Where(r => gameIds.Contains(r.GameId))
            .SelectMany(r => r.PendingPosts)
            .ProjectTo<PendingPost>(mapper.ConfigurationProvider)
            .ToArrayAsync();
    }

    /// <inheritdoc />
    public Task<GameExtended> GetGameDetails(Guid gameId, Guid userId)
    {
        return dbContext.Games
            .Where(AccessibilityFilters.GameAvailable(userId))
            .Where(g => g.GameId == gameId)
            .ProjectTo<GameExtended>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();
    }

    /// <inheritdoc />
    public Task<Game> GetGame(Guid gameId, Guid userId)
    {
        return dbContext.Games
            .Where(AccessibilityFilters.GameAvailable(userId))
            .Where(g => g.GameId == gameId)
            .ProjectTo<Game>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<GameTag>> GetTags()
    {
        return await dbContext.Tags
            .ProjectTo<GameTag>(mapper.ConfigurationProvider)
            .ToArrayAsync();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Game>> GetPopularGames(int gamesCount)
    {
        return await dbContext.Games
            .Where(g => !g.IsRemoved)
            .Where(g => g.Status == GameStatus.Active || g.Status == GameStatus.Requirement)
            .OrderByDescending(g => g.Readers.Count)
            .Take(gamesCount)
            .ProjectTo<Game>(mapper.ConfigurationProvider)
            .ToArrayAsync();
    }
}