using System;
using DM.Services.DataAccess.BusinessObjects.Games.Posts;

namespace DM.Services.Gaming.BusinessProcesses.Games.Creating
{
    /// <summary>
    /// Factory for room DAL model
    /// </summary>
    public interface IRoomFactory
    {
        /// <summary>
        /// Creates a DAL model for the default room in game
        /// </summary>
        /// <param name="gameId">Game identifier</param>
        /// <returns></returns>
        Room Create(Guid gameId);
    }
}