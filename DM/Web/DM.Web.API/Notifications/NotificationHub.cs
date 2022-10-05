using System;
using System.Linq;
using System.Threading.Tasks;
using DM.Web.Core.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace DM.Web.API.Notifications;

/// <inheritdoc />
public class NotificationHub : Hub<INotificationHub>
{
    private readonly IUserConnectionService connectionService;

    /// <inheritdoc />
    public NotificationHub(
        IUserConnectionService connectionService)
    {
        this.connectionService = connectionService;
    }

    /// <inheritdoc />
    public override Task OnConnectedAsync()
    {
        var (hasToken, token) = TryExtractAuthToken();
        if (hasToken)
        {
            connectionService.Add(token, Context.ConnectionId);
        }

        return base.OnConnectedAsync();
    }

    /// <inheritdoc />
    public override Task OnDisconnectedAsync(Exception exception)
    {
        var (hasToken, token) = TryExtractAuthToken();
        if (hasToken)
        {
            connectionService.Remove(token, Context.ConnectionId);
        }

        return base.OnDisconnectedAsync(exception);
    }

    private (bool success, string token) TryExtractAuthToken()
    {
        string token;
        if (!Context.GetHttpContext().Request.Query.TryGetValue("access_token", out var queryValues) ||
            !queryValues.Any() || string.IsNullOrEmpty(token = queryValues.First()))
        {
            return (false, null);
        }

        return (true, token);
    }
}