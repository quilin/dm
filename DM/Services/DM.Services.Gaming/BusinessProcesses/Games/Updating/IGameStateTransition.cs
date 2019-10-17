using DM.Services.Core.Dto.Enums;
using DM.Services.Gaming.Dto.Output;

namespace DM.Services.Gaming.BusinessProcesses.Games.Updating
{
    /// <summary>
    /// Resolving available status transitions
    /// </summary>
    public interface IGameStateTransition
    {
        /// <summary>
        /// Check if transition is possible and requires nanny assignment
        /// </summary>
        /// <param name="game"></param>
        /// <param name="targetStatus"></param>
        /// <returns></returns>
        (bool success, bool? assignNanny) TryChange(GameExtended game, GameStatus targetStatus);
    }
}