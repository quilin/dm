using System;
using System.Threading;
using System.Threading.Tasks;
using DM.Services.Core.Dto;

namespace DM.Services.Gaming.BusinessProcesses.Readers.Subscribing;

/// <summary>
/// Service for game subscribing
/// </summary>
public interface IReadingSubscribingService
{
    /// <summary>
    /// Subscribe to a game
    /// </summary>
    /// <param name="gameId">Game identifier</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<GeneralUser> Subscribe(Guid gameId, CancellationToken cancellationToken);

    /// <summary>
    /// Unsubscribe from a game
    /// </summary>
    /// <param name="gameId">Game identifier</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task Unsubscribe(Guid gameId, CancellationToken cancellationToken);
}