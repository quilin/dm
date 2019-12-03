using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Common.Authorization;
using DM.Services.Common.BusinessProcesses.UnreadCounters;
using DM.Services.Common.Extensions;
using DM.Services.Core.Dto;
using DM.Services.Core.Dto.Enums;
using DM.Services.Core.Exceptions;
using DM.Services.DataAccess.BusinessObjects.Common;
using DM.Services.Gaming.Authorization;
using DM.Services.Gaming.Dto.Output;
using Microsoft.Extensions.Caching.Memory;

namespace DM.Services.Gaming.BusinessProcesses.Games.Reading
{
    /// <inheritdoc />
    public class GameReadingService : IGameReadingService
    {
        private readonly IIntentionManager intentionManager;
        private readonly IGameReadingRepository repository;
        private readonly IUnreadCountersRepository unreadCountersRepository;
        private readonly IMemoryCache cache;
        private readonly IIdentity identity;

        private const string TagListCacheKey = nameof(TagListCacheKey);

        /// <inheritdoc />
        public GameReadingService(
            IIntentionManager intentionManager,
            IGameReadingRepository repository,
            IIdentityProvider identityProvider,
            IUnreadCountersRepository unreadCountersRepository,
            IMemoryCache cache)
        {
            this.intentionManager = intentionManager;
            this.repository = repository;
            this.unreadCountersRepository = unreadCountersRepository;
            this.cache = cache;
            identity = identityProvider.Current;
        }

        /// <inheritdoc />
        public Task<IEnumerable<GameTag>> GetTags()
        {
            return cache.GetOrCreateAsync(TagListCacheKey, e =>
            {
                e.SlidingExpiration = TimeSpan.FromDays(1);
                return repository.GetTags();
            });
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Game>> GetOwnGames()
        {
            var currentUserId = identity.User.UserId;
            var games = (await repository.GetOwn(currentUserId)).ToArray();

            await unreadCountersRepository.FillEntityCounters(
                games, currentUserId, g => g.Id, g => g.UnreadCommentsCount);
            await unreadCountersRepository.FillEntityCounters(
                games, currentUserId, g => g.Id, g => g.UnreadCharactersCount, UnreadEntryType.Character);

            var gameRooms = await repository.GetAvailableRoomIds(games.Select(g => g.Id), currentUserId);
            var allRoomIds = gameRooms.SelectMany(r => r.Value).ToArray();
            var unreadPostCounters = await unreadCountersRepository.SelectByEntities(
                currentUserId, UnreadEntryType.Message, allRoomIds);
            foreach (var game in games)
            {
                if (gameRooms.TryGetValue(game.Id, out var roomIds))
                {
                    game.UnreadPostsCount = roomIds.Sum(id =>
                        unreadPostCounters.TryGetValue(id, out var count) ? count : 0);
                }
            }

            return games;
        }

        /// <inheritdoc />
        public async Task<(IEnumerable<Game> games, PagingResult paging)> GetGames(PagingQuery query,
            GameStatus? status)
        {
            var currentUserId = identity.User.UserId;
            var totalCount = await repository.Count(status, currentUserId);
            var pagingData = new PagingData(query, identity.Settings.TopicsPerPage, totalCount);

            var games = (await repository.GetGames(pagingData, status, currentUserId)).ToArray();
            await unreadCountersRepository.FillEntityCounters(games, currentUserId,
                g => g.Id, g => g.UnreadCharactersCount, UnreadEntryType.Character);

            var gamesWithAvailableComments = games
                .Where(g => intentionManager.IsAllowed(GameIntention.ReadComments, g).Result)
                .ToArray();
            await unreadCountersRepository.FillEntityCounters(gamesWithAvailableComments, currentUserId,
                g => g.Id, g => g.UnreadCommentsCount);

            return (games, pagingData.Result);
        }

        /// <inheritdoc />
        public async Task<Game> GetGame(Guid gameId)
        {
            var currentUserId = identity.User.UserId;
            var game = await repository.GetGame(gameId, currentUserId);
            if (game == null)
            {
                throw new HttpException(HttpStatusCode.Gone, "Game not found");
            }

            await unreadCountersRepository.FillEntityCounters(new[] {game}, currentUserId,
                g => g.Id, g => g.UnreadCommentsCount);
            await unreadCountersRepository.FillEntityCounters(new[] {game}, currentUserId,
                g => g.Id, g => g.UnreadCharactersCount, UnreadEntryType.Character);

            return game;
        }

        /// <inheritdoc />
        public async Task<GameExtended> GetGameDetails(Guid gameId)
        {
            var currentUserId = identity.User.UserId;
            var game = await repository.GetGameDetails(gameId, currentUserId);
            if (game == null)
            {
                throw new HttpException(HttpStatusCode.Gone, "Game not found");
            }

            await unreadCountersRepository.FillEntityCounters(new[] {game}, currentUserId,
                g => g.Id, g => g.UnreadCommentsCount);
            await unreadCountersRepository.FillEntityCounters(new[] {game}, currentUserId,
                g => g.Id, g => g.UnreadCharactersCount, UnreadEntryType.Character);

            return game;
        }
    }
}