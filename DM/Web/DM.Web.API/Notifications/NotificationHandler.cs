using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DM.Services.MessageQueuing;
using DM.Services.Notifications.Dto;
using DM.Web.API.Dto.Notifications;
using DM.Web.Core.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace DM.Web.API.Notifications
{
    internal class NotificationHandler : IMessageHandler<RealtimeNotification>
    {
        private readonly IUserConnectionService connectionService;
        private readonly IHubContext<NotificationHub, INotificationHub> hubContext;

        public NotificationHandler(
            IUserConnectionService connectionService,
            IHubContext<NotificationHub, INotificationHub> hubContext)
        {
            this.connectionService = connectionService;
            this.hubContext = hubContext;
        }

        public async Task<ProcessResult> Handle(RealtimeNotification message, CancellationToken cancellationToken)
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
            return ProcessResult.Success;
        }
    }
}