using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DM.Services.Authentication.Implementation;

namespace DM.Web.Core.Hubs;

internal class UserConnectionService(IAuthenticationService authenticationService) : IUserConnectionService
{
    private static readonly ConcurrentDictionary<Guid, HashSet<string>> Connections = new();

    public async Task Add(string authToken, string connectionId)
    {
        var identity = await authenticationService.Authenticate(authToken);
        if (!identity.User.IsAuthenticated)
        {
            return;
        }

        Connections.AddOrUpdate(
            identity.User.UserId,
            _ => new HashSet<string> {connectionId},
            (_, connectionIds) =>
            {
                connectionIds.Add(connectionId);
                return connectionIds;
            });
    }

    public async Task Remove(string authToken, string connectionId)
    {
        var identity = await authenticationService.Authenticate(authToken);
        if (!identity.User.IsAuthenticated ||
            !Connections.TryGetValue(identity.User.UserId, out var connectionIds))
        {
            return;
        }

        connectionIds.Remove(connectionId);
        if (connectionIds.Count == 0)
        {
            Connections.TryRemove(identity.User.UserId, out _);
        }
    }

    public IReadOnlyDictionary<Guid, IEnumerable<string>> GetConnectedUsers() => 
        Connections.ToDictionary(
            k => k.Key,
            v => v.Value.Select(c => c));
}