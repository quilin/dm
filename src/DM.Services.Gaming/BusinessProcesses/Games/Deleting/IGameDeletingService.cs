using System;
using System.Threading;
using System.Threading.Tasks;

namespace DM.Services.Gaming.BusinessProcesses.Games.Deleting;

/// <summary>
/// Game deleting service
/// </summary>
public interface IGameDeletingService
{
    /// <summary>
    /// Remove existing game
    /// </summary>
    /// <param name="gameId">Game identifier</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task DeleteGame(Guid gameId, CancellationToken cancellationToken);
}