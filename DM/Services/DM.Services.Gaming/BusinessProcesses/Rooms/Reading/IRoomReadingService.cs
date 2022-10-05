using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Services.Gaming.Dto.Output;

namespace DM.Services.Gaming.BusinessProcesses.Rooms.Reading;

/// <summary>
/// Reading service for rooms
/// </summary>
public interface IRoomReadingService
{
    /// <summary>
    /// Get all available game rooms
    /// </summary>
    /// <param name="gameId">Game identifier</param>
    /// <returns></returns>
    Task<IEnumerable<Room>> GetAll(Guid gameId);

    /// <summary>
    /// Get single existing room
    /// </summary>
    /// <param name="roomId">Room identifier</param>
    /// <returns></returns>
    Task<Room> Get(Guid roomId);
}