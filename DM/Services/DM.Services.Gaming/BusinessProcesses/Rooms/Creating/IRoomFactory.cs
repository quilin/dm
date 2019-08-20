using DM.Services.DataAccess.BusinessObjects.Games.Posts;

namespace DM.Services.Gaming.BusinessProcesses.Rooms.Creating
{
    /// <summary>
    /// Factory for room DAL model
    /// </summary>
    public interface IRoomFactory
    {
        /// <summary>
        /// Create room out of DTO model
        /// </summary>
        /// <param name="game">Parent game</param>
        /// <param name="title">Room title</param>
        /// <returns></returns>
        Room Create(Dto.Output.Game game, string title);
    }
}