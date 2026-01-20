using System;
using System.Threading;
using System.Threading.Tasks;
using DM.Services.DataAccess.BusinessObjects.Games.Links;

namespace DM.Services.Gaming.BusinessProcesses.Readers.Subscribing;

/// <summary>
/// Storage for game subscriptions
/// </summary>
internal interface IReadingSubscribingRepository
{
    /// <summary>
    /// Check if user has a game subscription
    /// </summary>
    /// <param name="userId">User identifier</param>
    /// <param name="gameId">Game identifier</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> HasSubscription(Guid userId, Guid gameId, CancellationToken cancellationToken);

    /// <summary>
    /// Store game subscription
    /// </summary>
    /// <param name="reader"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task Add(Reader reader, CancellationToken cancellationToken);

    /// <summary>
    /// Remove stored subscription
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="gameId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task Delete(Guid userId, Guid gameId, CancellationToken cancellationToken);
}