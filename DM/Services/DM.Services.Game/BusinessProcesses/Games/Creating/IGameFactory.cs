using DM.Services.Game.Dto.Input;

namespace DM.Services.Game.BusinessProcesses.Games.Creating
{
    /// <summary>
    /// Factory for game DAL model
    /// </summary>
    public interface IGameFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="createGame"></param>
        /// <returns></returns>
        DataAccess.BusinessObjects.Games.Game Create(CreateGame createGame);
    }
}