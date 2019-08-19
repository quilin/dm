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
        /// <returns></returns>
        Task<int> Count(GameStatus? status);

        /// <summary>
        /// Get list of matching games on certain page
        /// </summary>
        /// <param name="pagingData">Paging data</param>
        /// <param name="status">Status filter</param>
        /// <returns></returns>
        Task<IEnumerable<Game>> Get(PagingData pagingData, GameStatus? status);

        /// <summary>
        /// Get single game
        /// </summary>
        /// <param name="gameId">Game identifier</param>
        /// <returns></returns>
        Task<GameExtended> Get(Guid gameId);
    }
}