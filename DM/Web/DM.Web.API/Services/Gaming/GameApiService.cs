using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DM.Services.Gaming.BusinessProcesses.Games.Creating;
using DM.Services.Gaming.BusinessProcesses.Games.Reading;
using DM.Services.Gaming.Dto.Input;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Games;
using Game = DM.Web.API.Dto.Games.Game;

namespace DM.Web.API.Services.Gaming
{
    /// <inheritdoc />
    public class GameApiService : IGameApiService
    {
        private readonly IGameReadingService readingService;
        private readonly IGameCreatingService creatingService;
        private readonly IMapper mapper;

        /// <inheritdoc />
        public GameApiService(
            IGameReadingService readingService,
            IGameCreatingService creatingService,
            IMapper mapper)
        {
            this.readingService = readingService;
            this.creatingService = creatingService;
            this.mapper = mapper;
        }

        /// <inheritdoc />
        public async Task<ListEnvelope<Game>> Get(GamesQuery gamesQuery)
        {
            var (games, paging) = await readingService.GetGames(gamesQuery, gamesQuery.Status?.FirstOrDefault());
            return new ListEnvelope<Game>(games.Select(mapper.Map<Game>), new Paging(paging));
        }

        /// <inheritdoc />
        public async Task<Envelope<Game>> Get(Guid gameId)
        {
            var game = await readingService.GetGame(gameId);
            return new Envelope<Game>(mapper.Map<Game>(game));
        }

        /// <inheritdoc />
        public async Task<Envelope<Game>> Create(Game game)
        {
            var createGame = mapper.Map<CreateGame>(game);
            var createdGame = await creatingService.Create(createGame);
            return new Envelope<Game>(mapper.Map<Game>(createdGame));
        }
    }
}