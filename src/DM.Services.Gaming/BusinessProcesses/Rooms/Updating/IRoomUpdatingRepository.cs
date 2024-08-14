using System;
using System.Threading.Tasks;
using DM.Services.DataAccess.RelationalStorage;
using DM.Services.Gaming.Dto.Internal;
using DM.Services.Gaming.Dto.Output;
using DbRoom = DM.Services.DataAccess.BusinessObjects.Games.Posts.Room;

namespace DM.Services.Gaming.BusinessProcesses.Rooms.Updating;

/// <summary>
/// Storage for room updates
/// </summary>
internal interface IRoomUpdatingRepository
{
    /// <summary>
    /// Get room for update
    /// </summary>
    /// <param name="roomId">Room identifier</param>
    /// <param name="userId">User identifier</param>
    /// <returns></returns>
    Task<RoomToUpdate> GetRoom(Guid roomId, Guid userId);

    /// <summary>
    /// Get room neighbours
    /// </summary>
    /// <param name="roomId"></param>
    /// <returns></returns>
    Task<RoomNeighbours> GetNeighbours(Guid roomId);

    /// <summary>
    /// Update room and its neighbours
    /// </summary>
    /// <param name="updateRoom"></param>
    /// <param name="updateOldPreviousRoom"></param>
    /// <param name="updateOldNextRoom"></param>
    /// <param name="updateNewPreviousRoom"></param>
    /// <param name="updateNewNextRoom"></param>
    /// <returns></returns>
    Task<Room> Update(
        IUpdateBuilder<DbRoom> updateRoom,
        IUpdateBuilder<DbRoom> updateOldNextRoom = null,
        IUpdateBuilder<DbRoom> updateOldPreviousRoom = null,
        IUpdateBuilder<DbRoom> updateNewNextRoom = null,
        IUpdateBuilder<DbRoom> updateNewPreviousRoom = null);

    /// <summary>
    /// Get first room of the game order info
    /// </summary>
    /// <param name="gameId">Game identifier</param>
    /// <returns></returns>
    Task<RoomOrderInfo> GetFirstRoomInfo(Guid gameId);
}