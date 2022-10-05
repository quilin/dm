using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Services.Gaming.Dto.Output;
using DbGame = DM.Services.DataAccess.BusinessObjects.Games.Game;
using DbRoom = DM.Services.DataAccess.BusinessObjects.Games.Posts.Room;
using DbTag = DM.Services.DataAccess.BusinessObjects.Games.Links.GameTag;

namespace DM.Services.Gaming.BusinessProcesses.Games.Creating;

/// <summary>
/// Storage for game creating
/// </summary>
internal interface IGameCreatingRepository
{
    /// <summary>
    /// Save new game and its first room
    /// </summary>
    /// <param name="game">Game DAL</param>
    /// <param name="room">Room DAL</param>
    /// <param name="tags">Game tag DALs</param>
    /// <returns></returns>
    Task<GameExtended> Create(DbGame game, DbRoom room, IEnumerable<DbTag> tags);

}