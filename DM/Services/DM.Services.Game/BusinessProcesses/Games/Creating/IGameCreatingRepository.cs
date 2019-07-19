using System.Threading.Tasks;
using DM.Services.Game.Dto.Output;

namespace DM.Services.Game.BusinessProcesses.Games.Creating
{
    /// <summary>
    /// Storage for game creating
    /// </summary>
    public interface IGameCreatingRepository
    {
        /// <summary>
        /// Save new game
        /// </summary>
        /// <param name="createGame"></param>
        /// <returns></returns>
        Task<GameExtended> Create(DataAccess.BusinessObjects.Games.Game createGame);
    }
}