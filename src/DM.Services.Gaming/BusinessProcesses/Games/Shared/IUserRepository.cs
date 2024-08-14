using System;
using System.Threading;
using System.Threading.Tasks;

namespace DM.Services.Gaming.BusinessProcesses.Games.Shared;

/// <summary>
/// User storage
/// </summary>
internal interface IUserRepository
{
    /// <summary>
    /// Try find user by login
    /// </summary>
    /// <param name="login">User login</param>
    /// <returns>Pair of existence flag and user identifier</returns>
    Task<(bool exists, Guid userId)> FindUserId(string login);

    /// <summary>
    /// User with login exists
    /// </summary>
    /// <param name="login">User login</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> UserExists(string login, CancellationToken cancellationToken);
}