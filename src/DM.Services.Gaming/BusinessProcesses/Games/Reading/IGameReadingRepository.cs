using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Services.Core.Dto;
using DM.Services.Gaming.Dto.Input;
using DM.Services.Gaming.Dto.Output;

namespace DM.Services.Gaming.BusinessProcesses.Games.Reading;

/// <summary>
/// Game reading storage
/// </summary>
internal interface IGameReadingRepository
{
    /// <summary>
    /// Count games that match the query
    /// </summary>
    /// <param name="status">Status filter</param>
    /// <param name="userId">User identifier</param>
    /// <returns></returns>
    Task<int> Count(GamesQuery status, Guid userId);

    /// <summary>
    /// Get list of matching games on certain page
    /// </summary>
    /// <param name="pagingData">Paging data</param>
    /// <param name="query">Filter query</param>
    /// <param name="userId">User identifier</param>
    /// <returns></returns>
    Task<IEnumerable<Game>> GetGames(PagingData pagingData, GamesQuery query, Guid userId);

    /// <summary>
    /// Get list of user owned active games
    /// </summary>
    /// <param name="userId">User identifier</param>
    /// <returns></returns>
    Task<IEnumerable<Game>> GetOwn(Guid userId);

    /// <summary>
    /// Get available room identifiers grouped by game ids
    /// </summary>
    /// <returns></returns>
    Task<IDictionary<Guid, IEnumerable<Guid>>> GetAvailableRoomIds(IEnumerable<Guid> gameIds, Guid userId);

    /// <summary>
    /// Get game pending posts
    /// </summary>
    /// <param name="gameIds">Game identifiers</param>
    /// <param name="userId">User identifier</param>
    /// <returns></returns>
    Task<IEnumerable<PendingPost>> GetPendingPosts(IEnumerable<Guid> gameIds, Guid userId);

    /// <summary>
    /// Get single game model
    /// </summary>
    /// <param name="gameId">Game identifier</param>
    /// <param name="userId">User identifier</param>
    /// <returns></returns>
    Task<Game> GetGame(Guid gameId, Guid userId);

    /// <summary>
    /// Get single game full info
    /// </summary>
    /// <param name="gameId">Game identifier</param>
    /// <param name="userId">User identifier</param>
    /// <returns></returns>
    Task<GameExtended> GetGameDetails(Guid gameId, Guid userId);

    /// <summary>
    /// Get list of available tags
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<GameTag>> GetTags();
        
    /// <summary>
    /// Get list of popular active games
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<Game>> GetPopularGames(int gamesCount);
}