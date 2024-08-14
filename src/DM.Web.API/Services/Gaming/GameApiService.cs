using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DM.Services.Gaming.BusinessProcesses.Games.Creating;
using DM.Services.Gaming.BusinessProcesses.Games.Deleting;
using DM.Services.Gaming.BusinessProcesses.Games.Reading;
using DM.Services.Gaming.BusinessProcesses.Games.Updating;
using DM.Services.Gaming.Dto.Input;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Games;
using Game = DM.Web.API.Dto.Games.Game;
using GameApiQuery = DM.Web.API.Dto.Games.GamesQuery;
using GamesQuery = DM.Services.Gaming.Dto.Input.GamesQuery;

namespace DM.Web.API.Services.Gaming;

/// <inheritdoc />
internal class GameApiService : IGameApiService
{
    private readonly IGameReadingService readingService;
    private readonly IGameCreatingService creatingService;
    private readonly IGameUpdatingService updatingService;
    private readonly IGameDeletingService deletingService;
    private readonly IMapper mapper;

    /// <inheritdoc />
    public GameApiService(
        IGameReadingService readingService,
        IGameCreatingService creatingService,
        IGameUpdatingService updatingService,
        IGameDeletingService deletingService,
        IMapper mapper)
    {
        this.readingService = readingService;
        this.creatingService = creatingService;
        this.updatingService = updatingService;
        this.deletingService = deletingService;
        this.mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<ListEnvelope<Game>> Get(GameApiQuery gamesQuery)
    {
        var query = mapper.Map<GamesQuery>(gamesQuery);
        var (games, paging) = await readingService.GetGames(query);
        return new ListEnvelope<Game>(games.Select(mapper.Map<Game>), new Paging(paging));
    }

    /// <inheritdoc />
    public async Task<ListEnvelope<Game>> GetOwn()
    {
        var games = await readingService.GetOwnGames();
        return new ListEnvelope<Game>(games.Select(mapper.Map<Game>));
    }

    /// <inheritdoc />
    public async Task<ListEnvelope<Game>> GetPopular()
    {
        var games = await readingService.GetPopularGames();
        return new ListEnvelope<Game>(games.Select(mapper.Map<Game>));
    }

    /// <inheritdoc />
    public async Task<Envelope<Game>> Get(Guid gameId)
    {
        var game = await readingService.GetGame(gameId);
        return new Envelope<Game>(mapper.Map<Game>(game));
    }

    /// <inheritdoc />
    public async Task<Envelope<Game>> GetDetails(Guid gameId)
    {
        var game = await readingService.GetGameDetails(gameId);
        return new Envelope<Game>(mapper.Map<Game>(game));
    }

    /// <inheritdoc />
    public async Task<Envelope<Game>> Create(Game game)
    {
        var createGame = mapper.Map<CreateGame>(game);
        var createdGame = await creatingService.Create(createGame);
        return new Envelope<Game>(mapper.Map<Game>(createdGame));
    }

    /// <inheritdoc />
    public async Task<Envelope<Game>> Update(Guid gameId, Game game)
    {
        var updateGame = mapper.Map<UpdateGame>(game);
        updateGame.GameId = gameId;
        var updatedGame = await updatingService.Update(updateGame);
        return new Envelope<Game>(mapper.Map<Game>(updatedGame));
    }

    /// <inheritdoc />
    public Task Delete(Guid gameId) => deletingService.DeleteGame(gameId);

    /// <inheritdoc />
    public async Task<ListEnvelope<Tag>> GetTags()
    {
        var tags = await readingService.GetTags();
        return new ListEnvelope<Tag>(tags.Select(mapper.Map<Tag>));
    }
}