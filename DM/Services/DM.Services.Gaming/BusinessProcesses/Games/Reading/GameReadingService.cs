using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Core.Dto;
using DM.Services.Core.Dto.Enums;
using DM.Services.Core.Exceptions;
using DM.Services.Gaming.Dto.Output;
using Microsoft.Extensions.Caching.Memory;

namespace DM.Services.Gaming.BusinessProcesses.Games.Reading
{
    /// <inheritdoc />
    public class GameReadingService : IGameReadingService
    {
        private readonly IGameReadingRepository repository;
        private readonly IMemoryCache cache;
        private readonly IIdentity identity;

        private const string TagListCacheKey = nameof(TagListCacheKey);

        /// <inheritdoc />
        public GameReadingService(
            IGameReadingRepository repository,
            IIdentityProvider identityProvider,
            IMemoryCache cache)
        {
            this.repository = repository;
            this.cache = cache;
            identity = identityProvider.Current;
        }

        /// <inheritdoc />
        public Task<IEnumerable<GameTag>> GetTags()
        {
            return cache.GetOrCreateAsync(TagListCacheKey, _ =>
                repository.GetTags());
        }

        /// <inheritdoc />
        public async Task<(IEnumerable<Game> games, PagingResult paging)> GetGames(PagingQuery query, GameStatus? status)
        {
            var totalCount = await repository.Count(status, identity.User.UserId);
            var pagingData = new PagingData(query, identity.Settings.TopicsPerPage, totalCount);

            var games = await repository.GetGames(pagingData, status, identity.User.UserId);
            return (games, pagingData.Result);
        }

        /// <inheritdoc />
        public async Task<Game> GetGame(Guid gameId)
        {
            var game = await repository.GetGame(gameId, identity.User.UserId);
            if (game == null)
            {
                throw new HttpException(HttpStatusCode.Gone, "Game not found");
            }

            return game;
        }

        /// <inheritdoc />
        public async Task<GameExtended> GetGameDetails(Guid gameId)
        {
            var game = await repository.GetGameDetails(gameId, identity.User.UserId);
            if (game == null)
            {
                throw new HttpException(HttpStatusCode.Gone, "Game not found");
            }

            return game;
        }
    }
}