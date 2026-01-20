using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Common.Authorization;
using DM.Services.Common.BusinessProcesses.UnreadCounters;
using DM.Services.Common.Extensions;
using DM.Services.Core.Dto;
using DM.Services.Core.Exceptions;
using DM.Services.DataAccess.BusinessObjects.Common;
using DM.Services.Gaming.Authorization;
using DM.Services.Gaming.BusinessProcesses.Schemas.Reading;
using DM.Services.Gaming.Dto.Input;
using DM.Services.Gaming.Dto.Output;
using FluentValidation;
using Microsoft.Extensions.Caching.Memory;

namespace DM.Services.Gaming.BusinessProcesses.Games.Reading;

/// <inheritdoc />
internal class GameReadingService(
    IValidator<GamesQuery> validator,
    IIntentionManager intentionManager,
    ISchemaReadingService schemaReadingService,
    IGameReadingRepository repository,
    IIdentityProvider identityProvider,
    IUnreadCountersRepository unreadCountersRepository,
    IMemoryCache cache) : IGameReadingService
{
    private const string TagListCacheKey = nameof(TagListCacheKey);
    private const string PopularGamesCacheKey = nameof(PopularGamesCacheKey);
    private const int PopularGamesLimit = 10;

    /// <inheritdoc />
    public Task<IEnumerable<GameTag>> GetTags(CancellationToken cancellationToken)
    {
        return cache.GetOrCreateAsync(TagListCacheKey, async e =>
        {
            e.SlidingExpiration = TimeSpan.FromDays(1);
            return await repository.GetTags(cancellationToken);
        });
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Game>> GetOwnGames(CancellationToken cancellationToken)
    {
        var currentUserId = identityProvider.Current.User.UserId;
        var games = (await repository.GetOwn(currentUserId, cancellationToken)).ToArray();

        await unreadCountersRepository.FillEntityCounters(games, currentUserId,
            g => g.Id, g => g.UnreadCommentsCount, UnreadEntryType.Message, cancellationToken);
        await unreadCountersRepository.FillEntityCounters(games, currentUserId,
            g => g.Id, g => g.UnreadCharactersCount, UnreadEntryType.Character, cancellationToken);

        var gameIds = games.Select(g => g.Id).ToArray();
        var gameRooms = await repository.GetAvailableRoomIds(gameIds, currentUserId, cancellationToken);
        var allRoomIds = gameRooms.SelectMany(r => r.Value).ToArray();
        var unreadPostCounters = await unreadCountersRepository.SelectByEntities(
            currentUserId, UnreadEntryType.Message, allRoomIds, cancellationToken);

        var pendingPosts = (await repository.GetPendingPosts(gameIds, currentUserId, cancellationToken)).ToArray();

        foreach (var game in games)
        {
            if (!gameRooms.TryGetValue(game.Id, out var roomIds)) continue;
            var gameRoomIds = roomIds.ToArray();
            game.UnreadPostsCount = gameRoomIds.Sum(id =>
                unreadPostCounters.TryGetValue(id, out var count) ? count : 0);
            game.Pendings = pendingPosts.Where(p => gameRoomIds.Contains(p.RoomId));
        }

        return games;
    }

    /// <inheritdoc />
    public async Task<(IEnumerable<Game> games, PagingResult paging)> GetGames(
        GamesQuery query, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(query, cancellationToken);

        var currentUserId = identityProvider.Current.User.UserId;
        var totalCount = await repository.Count(query, currentUserId, cancellationToken);
        var pagingData = new PagingData(query,
            identityProvider.Current.Settings.Paging.EntitiesPerPage, totalCount);

        var games = (await repository.GetGames(pagingData, query, currentUserId, cancellationToken)).ToArray();
        await unreadCountersRepository.FillEntityCounters(games, currentUserId,
            g => g.Id, g => g.UnreadCharactersCount, UnreadEntryType.Character, cancellationToken);

        var gamesWithAvailableComments = games
            .Where(g => intentionManager.IsAllowed(GameIntention.ReadComments, g))
            .ToArray();
        await unreadCountersRepository.FillEntityCounters(gamesWithAvailableComments, currentUserId,
            g => g.Id, g => g.UnreadCommentsCount, UnreadEntryType.Message, cancellationToken);

        return (games, pagingData.Result);
    }

    /// <inheritdoc />
    public async Task<Game> GetGame(Guid gameId, CancellationToken cancellationToken)
    {
        var currentUserId = identityProvider.Current.User.UserId;
        var game = await repository.GetGame(gameId, currentUserId, cancellationToken);
        if (game == null)
        {
            throw new HttpException(HttpStatusCode.Gone, "Game not found");
        }

        await unreadCountersRepository.FillEntityCounters(new[] {game}, currentUserId,
            g => g.Id, g => g.UnreadCommentsCount, UnreadEntryType.Message, cancellationToken);
        await unreadCountersRepository.FillEntityCounters(new[] {game}, currentUserId,
            g => g.Id, g => g.UnreadCharactersCount, UnreadEntryType.Character, cancellationToken);

        return game;
    }

    /// <inheritdoc />
    public async Task<GameExtended> GetGameDetails(Guid gameId, CancellationToken cancellationToken)
    {
        var currentUserId = identityProvider.Current.User.UserId;
        var game = await repository.GetGameDetails(gameId, currentUserId, cancellationToken);
        if (game == null)
        {
            throw new HttpException(HttpStatusCode.Gone, "Game not found");
        }

        if (game.AttributeSchemaId.HasValue)
        {
            game.AttributeSchema = await schemaReadingService.Get(game.AttributeSchemaId.Value, cancellationToken);
        }

        await unreadCountersRepository.FillEntityCounters(new[] {game}, currentUserId,
            g => g.Id, g => g.UnreadCommentsCount, UnreadEntryType.Message, cancellationToken);
        await unreadCountersRepository.FillEntityCounters(new[] {game}, currentUserId,
            g => g.Id, g => g.UnreadCharactersCount, UnreadEntryType.Character, cancellationToken);

        return game;
    }

    /// <param name="cancellationToken"></param>
    /// <inheritdoc />
    public Task<IEnumerable<Game>> GetPopularGames(CancellationToken cancellationToken)
    {
        return cache.GetOrCreateAsync(PopularGamesCacheKey, async e =>
        {
            e.SlidingExpiration = TimeSpan.FromDays(1);
            return await repository.GetPopularGames(PopularGamesLimit, cancellationToken);
        });
    }
}