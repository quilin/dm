using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Services.Core.Dto;
using DM.Services.Core.Dto.Enums;
using DM.Services.Gaming.Dto.Output;

namespace DM.Services.Gaming.BusinessProcesses.Games.Reading
{
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
        /// Get games page
        /// </summary>
        /// <param name="query">Paging query</param>
        /// <param name="status">Status filter</param>
        /// <returns>List of fetched games and paging data</returns>
        Task<(IEnumerable<Game> games, PagingResult paging)> GetGames(PagingQuery query, GameStatus? status);

        /// <summary>
        /// Get game by identifier
        /// </summary>
        /// <param name="gameId">Game identifier</param>
        /// <returns></returns>
        Task<GameExtended> GetGame(Guid gameId);
    }
}