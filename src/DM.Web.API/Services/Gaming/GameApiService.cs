using System;
using System.Linq;
using System.Threading;
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
internal class GameApiService(
    IGameReadingService readingService,
    IGameCreatingService creatingService,
    IGameUpdatingService updatingService,
    IGameDeletingService deletingService,
    IMapper mapper) : IGameApiService
{
    /// <inheritdoc />
    public async Task<ListEnvelope<Game>> Get(GameApiQuery gamesQuery, CancellationToken cancellationToken)
    {
        var query = mapper.Map<GamesQuery>(gamesQuery);
        var (games, paging) = await readingService.GetGames(query, cancellationToken);
        return new ListEnvelope<Game>(games.Select(mapper.Map<Game>), new Paging(paging));
    }

    /// <param name="cancellationToken"></param>
    /// <inheritdoc />
    public async Task<ListEnvelope<Game>> GetOwn(CancellationToken cancellationToken)
    {
        var games = await readingService.GetOwnGames(cancellationToken);
        return new ListEnvelope<Game>(games.Select(mapper.Map<Game>));
    }

    /// <param name="cancellationToken"></param>
    /// <inheritdoc />
    public async Task<ListEnvelope<Game>> GetPopular(CancellationToken cancellationToken)
    {
        var games = await readingService.GetPopularGames(cancellationToken);
        return new ListEnvelope<Game>(games.Select(mapper.Map<Game>));
    }

    /// <inheritdoc />
    public async Task<Envelope<Game>> Get(Guid gameId, CancellationToken cancellationToken)
    {
        var game = await readingService.GetGame(gameId, cancellationToken);
        return new Envelope<Game>(mapper.Map<Game>(game));
    }

    /// <inheritdoc />
    public async Task<Envelope<Game>> GetDetails(Guid gameId, CancellationToken cancellationToken)
    {
        var game = await readingService.GetGameDetails(gameId, cancellationToken);
        return new Envelope<Game>(mapper.Map<Game>(game));
    }

    /// <inheritdoc />
    public async Task<Envelope<Game>> Create(Game game, CancellationToken cancellationToken)
    {
        var createGame = mapper.Map<CreateGame>(game);
        var createdGame = await creatingService.Create(createGame, cancellationToken);
        return new Envelope<Game>(mapper.Map<Game>(createdGame));
    }

    /// <inheritdoc />
    public async Task<Envelope<Game>> Update(Guid gameId, Game game, CancellationToken cancellationToken)
    {
        var updateGame = mapper.Map<UpdateGame>(game);
        updateGame.GameId = gameId;
        var updatedGame = await updatingService.Update(updateGame, cancellationToken);
        return new Envelope<Game>(mapper.Map<Game>(updatedGame));
    }

    /// <inheritdoc />
    public Task Delete(Guid gameId, CancellationToken cancellationToken) =>
        deletingService.DeleteGame(gameId, cancellationToken);

    /// <param name="cancellationToken"></param>
    /// <inheritdoc />
    public async Task<ListEnvelope<Tag>> GetTags(CancellationToken cancellationToken)
    {
        var tags = await readingService.GetTags(cancellationToken);
        return new ListEnvelope<Tag>(tags.Select(mapper.Map<Tag>));
    }
}