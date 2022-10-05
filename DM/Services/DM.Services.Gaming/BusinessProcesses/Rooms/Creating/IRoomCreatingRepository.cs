using System;
using System.Threading.Tasks;
using DM.Services.DataAccess.RelationalStorage;
using DM.Services.Gaming.Dto.Internal;
using DM.Services.Gaming.Dto.Output;
using DbRoom = DM.Services.DataAccess.BusinessObjects.Games.Posts.Room;

namespace DM.Services.Gaming.BusinessProcesses.Rooms.Creating;

/// <summary>
/// Game room creating storage
/// </summary>
internal interface IRoomCreatingRepository
{
    /// <summary>
    /// Create new room
    /// </summary>
    /// <param name="room"></param>
    /// <param name="updateLastRoom"></param>
    /// <returns></returns>
    Task<Room> Create(DbRoom room, IUpdateBuilder<DbRoom> updateLastRoom);

    /// <summary>
    /// Find last room in game
    /// </summary>
    /// <param name="gameId">Game identifier</param>
    /// <returns></returns>
    Task<RoomOrderInfo> GetLastRoomInfo(Guid gameId);
}