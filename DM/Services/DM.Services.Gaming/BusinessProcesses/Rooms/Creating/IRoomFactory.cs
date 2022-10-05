using System;
using DM.Services.DataAccess.BusinessObjects.Games.Posts;
using DM.Services.Gaming.Dto.Input;

namespace DM.Services.Gaming.BusinessProcesses.Rooms.Creating;

/// <summary>
/// Factory for room DAL model
/// </summary>
internal interface IRoomFactory
{
    /// <summary>
    /// Create room out of DTO model
    /// </summary>
    /// <param name="createRoom">DTO model</param>
    /// <returns></returns>
    Room CreateFirst(CreateRoom createRoom);

    /// <summary>
    /// Create new room after given room
    /// </summary>
    /// <param name="createRoom">DTO for creating</param>
    /// <param name="lastRoomId">Last room identifier</param>
    /// <param name="lastRoomOrderNumber">Last room order number</param>
    /// <returns></returns>
    Room CreateAfter(CreateRoom createRoom, Guid lastRoomId, double lastRoomOrderNumber);
}