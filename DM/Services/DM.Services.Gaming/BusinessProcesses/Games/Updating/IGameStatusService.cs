using System;
using System.Threading.Tasks;
using DM.Services.Core.Dto.Enums;
using DM.Services.Gaming.Dto.Output;

namespace DM.Services.Gaming.BusinessProcesses.Games.Updating
{
    /// <summary>
    /// Game status change service
    /// </summary>
    public interface IGameStatusService
    {
        /// <summary>
        /// Update game status
        /// </summary>
        /// <param name="gameId">Game identifier</param>
        /// <param name="gameStatus">Target status</param>
        /// <returns></returns>
        Task<GameExtended> UpdateStatus(Guid gameId, GameStatus gameStatus);
    }
}