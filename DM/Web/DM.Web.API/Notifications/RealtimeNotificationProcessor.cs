﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DM.Services.Notifications.Dto;
using DM.Web.API.Dto.Notifications;
using DM.Web.Core.Hubs;
using Jamq.Client.Abstractions.Consuming;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace DM.Web.API.Notifications
{
    internal class RealtimeNotificationProcessor : IProcessor<string, RealtimeNotification>
    {
        private readonly ILogger<RealtimeNotificationProcessor> logger;
        private readonly IUserConnectionService connectionService;
        private readonly IHubContext<NotificationHub, INotificationHub> hubContext;

        public RealtimeNotificationProcessor(
            ILogger<RealtimeNotificationProcessor> logger,
            IUserConnectionService connectionService,
            IHubContext<NotificationHub, INotificationHub> hubContext)
        {
            this.logger = logger;
            this.connectionService = connectionService;
            this.hubContext = hubContext;
        }

        public async Task<ProcessResult> Process(
            string key, RealtimeNotification message, CancellationToken cancellationToken)
        {
            var connectedUsers = connectionService.GetConnectedUsers();
            var connectionIds = message.RecipientIds
                .Where(connectedUsers.ContainsKey)
                .SelectMany(id => connectedUsers[id])
                .ToArray();
            await hubContext.Clients.Clients(connectionIds).Send(new Notification
            {
                Id = message.NotificationId,
                EventType = message.EventType,
                Payload = message.Metadata
            });
            logger.LogDebug("Message published: {Message} for users {Users}", message.Metadata, message.RecipientIds);
            return ProcessResult.Success;
        }
    }
}