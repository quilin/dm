using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Services.DataAccess.BusinessObjects.Games.Links;
using DM.Services.Gaming.Dto.Output;
using DbGame = DM.Services.DataAccess.BusinessObjects.Games.Game;
using DbRoom = DM.Services.DataAccess.BusinessObjects.Games.Posts.Room;

namespace DM.Services.Gaming.BusinessProcesses.Games.Creating
{
    /// <summary>
    /// Storage for game creating
    /// </summary>
    public interface IGameCreatingRepository
    {
        /// <summary>
        /// Save new game and its first room
        /// </summary>
        /// <param name="game">Game DAL</param>
        /// <param name="room">Room DAL</param>
        /// <param name="tags">Game tag DALs</param>
        /// <returns></returns>
        Task<GameExtended> Create(DbGame game, DbRoom room, IEnumerable<GameTag> tags);

        /// <summary>
        /// Try find user by login
        /// </summary>
        /// <param name="login">User login</param>
        /// <returns>Pair of existence flag and user identifier</returns>
        Task<(bool exists, Guid userId)> FindUserId(string login);
    }
}