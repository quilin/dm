using System;
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
    /// <returns></returns>
    Task DeleteGame(Guid gameId);
}