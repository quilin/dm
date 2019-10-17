using System.Threading.Tasks;
using DM.Services.DataAccess.BusinessObjects.Users;
using DM.Services.DataAccess.RelationalStorage;
using DM.Services.Gaming.Dto.Output;
using Game = DM.Services.DataAccess.BusinessObjects.Games.Game;

namespace DM.Services.Gaming.BusinessProcesses.Games.Updating
{
    /// <summary>
    /// Game updating storage
    /// </summary>
    public interface IGameUpdatingRepository
    {
        /// <summary>
        /// Save game changes
        /// </summary>
        /// <param name="updateGame">Game changes</param>
        /// <param name="assistantAssignmentToken"></param>
        /// <returns></returns>
        Task<GameExtended> Update(IUpdateBuilder<Game> updateGame, Token assistantAssignmentToken);
    }
}