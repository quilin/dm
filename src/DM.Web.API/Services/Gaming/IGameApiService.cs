using System;
using System.Threading;
using System.Threading.Tasks;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Games;
using GamesQuery = DM.Web.API.Dto.Games.GamesQuery;

namespace DM.Web.API.Services.Gaming;

/// <summary>
/// API service for game resources
/// </summary>
public interface IGameApiService
{
    /// <summary>
    /// Get list of searched games
    /// </summary>
    /// <param name="gamesQuery">Search query</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Envelope for games list</returns>
    Task<ListEnvelope<Game>> Get(GamesQuery gamesQuery, CancellationToken cancellationToken);

    /// <summary>
    /// Get user owned games
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<ListEnvelope<Game>> GetOwn(CancellationToken cancellationToken);

    /// <summary>
    /// Get most popular games
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<ListEnvelope<Game>> GetPopular(CancellationToken cancellationToken);

    /// <summary>
    /// Get certain game
    /// </summary>
    /// <param name="gameId">Game identifier</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Envelope<Game>> Get(Guid gameId, CancellationToken cancellationToken);

    /// <summary>
    /// Get certain game details
    /// </summary>
    /// <param name="gameId">Game identifier</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Envelope<Game>> GetDetails(Guid gameId, CancellationToken cancellationToken);

    /// <summary>
    /// Create new game
    /// </summary>
    /// <param name="game">Game API model</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Envelope for created game</returns>
    Task<Envelope<Game>> Create(Game game, CancellationToken cancellationToken);

    /// <summary>
    /// Update existing game
    /// </summary>
    /// <param name="gameId">Game identifier</param>
    /// <param name="game">Game API model</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Envelope for updated game</returns>
    Task<Envelope<Game>> Update(Guid gameId, Game game, CancellationToken cancellationToken);

    /// <summary>
    /// Delete existing game
    /// </summary>
    /// <param name="gameId">Game identifier</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task Delete(Guid gameId, CancellationToken cancellationToken);

    /// <summary>
    /// Get all available game tags
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<ListEnvelope<Tag>> GetTags(CancellationToken cancellationToken);
}