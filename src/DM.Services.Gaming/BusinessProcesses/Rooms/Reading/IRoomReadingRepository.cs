using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Services.Gaming.Dto.Output;

namespace DM.Services.Gaming.BusinessProcesses.Rooms.Reading;

/// <summary>
/// Storage for reading rooms
/// </summary>
internal interface IRoomReadingRepository
{
    /// <summary>
    /// Get list of available rooms in game
    /// </summary>
    /// <param name="gameId">Game identifier</param>
    /// <param name="userId">Authenticated user identifier</param>
    /// <returns></returns>
    Task<IEnumerable<Room>> GetAllAvailable(Guid gameId, Guid userId);

    /// <summary>
    /// Get single existing available room
    /// </summary>
    /// <param name="roomId">Room identifier</param>
    /// <param name="userId">User identifier</param>
    /// <returns></returns>
    Task<Room> GetAvailable(Guid roomId, Guid userId);
}