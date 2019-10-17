using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Services.Core.Dto;
using DM.Services.Core.Dto.Enums;
using DM.Services.Gaming.Dto.Output;

namespace DM.Services.Gaming.BusinessProcesses.Games.Reading
{
    /// <summary>
    /// Game reading storage
    /// </summary>
    public interface IGameReadingRepository
    {
        /// <summary>
        /// Count games that match the query
        /// </summary>
        /// <param name="status">Status filter</param>
        /// <param name="userId">User identifier</param>
        /// <returns></returns>
        Task<int> Count(GameStatus? status, Guid userId);

        /// <summary>
        /// Get list of matching games on certain page
        /// </summary>
        /// <param name="pagingData">Paging data</param>
        /// <param name="status">Status filter</param>
        /// <param name="userId">User identifier</param>
        /// <returns></returns>
        Task<IEnumerable<Game>> GetGames(PagingData pagingData, GameStatus? status, Guid userId);

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
    }
}