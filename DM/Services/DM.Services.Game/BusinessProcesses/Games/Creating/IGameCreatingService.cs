using System.Threading.Tasks;
using DM.Services.Game.Dto.Input;
using DM.Services.Game.Dto.Output;

namespace DM.Services.Game.BusinessProcesses.Games.Creating
{
    /// <summary>
    /// Service to create games
    /// </summary>
    public interface IGameCreatingService
    {
        /// <summary>
        /// Create new game
        /// </summary>
        /// <returns></returns>
        Task<GameExtended> Create(CreateGame createGame);
    }
}