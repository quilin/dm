using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DM.Web.Core.Hubs;

/// <summary>
/// SignalR connected users service
/// </summary>
public interface IUserConnectionService
{
    /// <summary>
    /// Authenticate user and save its connection
    /// </summary>
    /// <param name="authToken"></param>
    /// <param name="connectionId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task Add(string authToken, string connectionId, CancellationToken cancellationToken);

    /// <summary>
    /// Authenticate user and remove its connection
    /// </summary>
    /// <param name="authToken"></param>
    /// <param name="connectionId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task Remove(string authToken, string connectionId, CancellationToken cancellationToken);

    /// <summary>
    /// Get all connected users
    /// </summary>
    /// <returns></returns>
    IReadOnlyDictionary<Guid, IEnumerable<string>> GetConnectedUsers();
}