using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Services.Core.Dto;
using DM.Services.Gaming.Dto.Input;
using DM.Services.Gaming.Dto.Output;

namespace DM.Services.Gaming.BusinessProcesses.Games.Reading;

/// <summary>
/// Service for reading games
/// </summary>
public interface IGameReadingService
{
    /// <summary>
    /// Get available game tags
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<GameTag>> GetTags();

    /// <summary>
    /// Get user games
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<Game>> GetOwnGames();

    /// <summary>
    /// Get games page
    /// </summary>
    /// <param name="query">Search query</param>
    /// <returns>List of fetched games and paging data</returns>
    Task<(IEnumerable<Game> games, PagingResult paging)> GetGames(GamesQuery query);

    /// <summary>
    /// Get game by identifier
    /// </summary>
    /// <param name="gameId">Game identifier</param>
    /// <returns></returns>
    Task<Game> GetGame(Guid gameId);

    /// <summary>
    /// Get game details by identifier
    /// </summary>
    /// <param name="gameId">Game identifier</param>
    /// <returns></returns>
    Task<GameExtended> GetGameDetails(Guid gameId);

    /// <summary>
    /// Get user games
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<Game>> GetPopularGames();
}