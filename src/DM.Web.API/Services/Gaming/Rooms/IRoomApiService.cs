using System;
using System.Threading.Tasks;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Games;

namespace DM.Web.API.Services.Gaming.Rooms;

/// <summary>
/// API service for game rooms
/// </summary>
public interface IRoomApiService
{
    /// <summary>
    /// Get list of all available game rooms
    /// </summary>
    /// <param name="gameId">Game identifier</param>
    /// <returns></returns>
    Task<ListEnvelope<Room>> GetAll(Guid gameId);

    /// <summary>
    /// Get single room
    /// </summary>
    /// <param name="roomId">Room identifier</param>
    /// <returns></returns>
    Task<Envelope<Room>> Get(Guid roomId);

    /// <summary>
    /// Create new room
    /// </summary>
    /// <param name="gameId">Game identifier</param>
    /// <param name="room">Room model</param>
    /// <returns></returns>
    Task<Envelope<Room>> Create(Guid gameId, Room room);

    /// <summary>
    /// Update existing room
    /// </summary>
    /// <param name="roomId">Room identifier</param>
    /// <param name="room">Room model</param>
    /// <returns></returns>
    Task<Envelope<Room>> Update(Guid roomId, Room room);

    /// <summary>
    /// Delete existing room
    /// </summary>
    /// <param name="roomId">Room identifier</param>
    /// <returns></returns>
    Task Delete(Guid roomId);
}