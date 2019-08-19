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

namespace DM.Services.Gaming.BusinessProcesses.Games.Reading
{
    /// <inheritdoc />
    public class GameReadingService : IGameReadingService
    {
        private readonly IGameReadingRepository repository;
        private readonly IIdentity identity;

        /// <inheritdoc />
        public GameReadingService(
            IGameReadingRepository repository,
            IIdentityProvider identityProvider)
        {
            this.repository = repository;
            identity = identityProvider.Current;
        }
        
        /// <inheritdoc />
        public async Task<(IEnumerable<Game> games, PagingResult paging)> GetGames(PagingQuery query, GameStatus? status)
        {
            var totalCount = await repository.Count(status);
            var pagingData = new PagingData(query, identity.Settings.TopicsPerPage, totalCount);

            var games = await repository.Get(pagingData, status);
            return (games, pagingData.Result);
        }

        /// <inheritdoc />
        public async Task<GameExtended> GetGame(Guid gameId)
        {
            var game = await repository.Get(gameId);
            if (game == null)
            {
                throw new HttpException(HttpStatusCode.Gone, "Game not found");
            }

            return game;
        }
    }
}