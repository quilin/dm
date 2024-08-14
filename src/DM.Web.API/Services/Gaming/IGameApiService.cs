using System;
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
    /// <returns>Envelope for games list</returns>
    Task<ListEnvelope<Game>> Get(GamesQuery gamesQuery);

    /// <summary>
    /// Get user owned games
    /// </summary>
    /// <returns></returns>
    Task<ListEnvelope<Game>> GetOwn();

    /// <summary>
    /// Get most popular games
    /// </summary>
    /// <returns></returns>
    Task<ListEnvelope<Game>> GetPopular();

    /// <summary>
    /// Get certain game
    /// </summary>
    /// <param name="gameId">Game identifier</param>
    /// <returns></returns>
    Task<Envelope<Game>> Get(Guid gameId);

    /// <summary>
    /// Get certain game details
    /// </summary>
    /// <param name="gameId">Game identifier</param>
    /// <returns></returns>
    Task<Envelope<Game>> GetDetails(Guid gameId);

    /// <summary>
    /// Create new game
    /// </summary>
    /// <param name="game">Game API model</param>
    /// <returns>Envelope for created game</returns>
    Task<Envelope<Game>> Create(Game game);

    /// <summary>
    /// Update existing game
    /// </summary>
    /// <param name="gameId">Game identifier</param>
    /// <param name="game">Game API model</param>
    /// <returns>Envelope for updated game</returns>
    Task<Envelope<Game>> Update(Guid gameId, Game game);

    /// <summary>
    /// Delete existing game
    /// </summary>
    /// <param name="gameId">Game identifier</param>
    /// <returns></returns>
    Task Delete(Guid gameId);

    /// <summary>
    /// Get all available game tags
    /// </summary>
    /// <returns></returns>
    Task<ListEnvelope<Tag>> GetTags();
}