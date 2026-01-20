using System;
using System.Threading;
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
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<ListEnvelope<Room>> GetAll(Guid gameId, CancellationToken cancellationToken);

    /// <summary>
    /// Get single room
    /// </summary>
    /// <param name="roomId">Room identifier</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Envelope<Room>> Get(Guid roomId, CancellationToken cancellationToken);

    /// <summary>
    /// Create new room
    /// </summary>
    /// <param name="gameId">Game identifier</param>
    /// <param name="room">Room model</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Envelope<Room>> Create(Guid gameId, Room room, CancellationToken cancellationToken);

    /// <summary>
    /// Update existing room
    /// </summary>
    /// <param name="roomId">Room identifier</param>
    /// <param name="room">Room model</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Envelope<Room>> Update(Guid roomId, Room room, CancellationToken cancellationToken);

    /// <summary>
    /// Delete existing room
    /// </summary>
    /// <param name="roomId">Room identifier</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task Delete(Guid roomId, CancellationToken cancellationToken);
}