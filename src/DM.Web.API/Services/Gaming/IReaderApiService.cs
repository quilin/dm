using System;
using System.Threading;
using System.Threading.Tasks;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Users;

namespace DM.Web.API.Services.Gaming;

/// <summary>
/// API service for game subscribers
/// </summary>
public interface IReaderApiService
{
    /// <summary>
    /// Get list of game readers
    /// </summary>
    /// <returns></returns>
    Task<ListEnvelope<User>> Get(Guid gameId, CancellationToken cancellationToken);

    /// <summary>
    /// Subscribe to a game as a reader
    /// </summary>
    /// <param name="gameId">Game identifier</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Envelope<User>> Subscribe(Guid gameId, CancellationToken cancellationToken);

    /// <summary>
    /// Unsubscribe from a game
    /// </summary>
    /// <param name="gameId">Game identifier</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task Unsubscribe(Guid gameId, CancellationToken cancellationToken);
}