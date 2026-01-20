using System;
using System.Threading;
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
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<RoomToUpdate> GetRoom(Guid roomId, Guid userId, CancellationToken cancellationToken);

    /// <summary>
    /// Get room neighbours
    /// </summary>
    /// <param name="roomId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<RoomNeighbours> GetNeighbours(Guid roomId, CancellationToken cancellationToken);

    /// <summary>
    /// Update room and its neighbours
    /// </summary>
    /// <param name="updateRoom"></param>
    /// <param name="updateOldNextRoom"></param>
    /// <param name="updateOldPreviousRoom"></param>
    /// <param name="updateNewNextRoom"></param>
    /// <param name="updateNewPreviousRoom"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Room> Update(IUpdateBuilder<DbRoom> updateRoom,
        IUpdateBuilder<DbRoom> updateOldNextRoom,
        IUpdateBuilder<DbRoom> updateOldPreviousRoom,
        IUpdateBuilder<DbRoom> updateNewNextRoom,
        IUpdateBuilder<DbRoom> updateNewPreviousRoom,
        CancellationToken cancellationToken);

    /// <summary>
    /// Get first room of the game order info
    /// </summary>
    /// <param name="gameId">Game identifier</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<RoomOrderInfo> GetFirstRoomInfo(Guid gameId, CancellationToken cancellationToken);
}