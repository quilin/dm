using System.Linq;
using System.Threading.Tasks;
using DM.Services.MessageQueuing;
using DM.Services.MessageQueuing.Processing;
using DM.Services.Notifications.Dto;
using DM.Web.Core.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace DM.Web.API.Notifications
{
    internal class NotificationProcessor : IMessageProcessor<RealtimeNotification>
    {
        private readonly IUserConnectionService connectionService;
        private readonly IHubContext<NotificationHub, INotificationHub> hubContext;

        public NotificationProcessor(
            IUserConnectionService connectionService,
            IHubContext<NotificationHub, INotificationHub> hubContext)
        {
            this.connectionService = connectionService;
            this.hubContext = hubContext;
        }

        public async Task<ProcessResult> Process(RealtimeNotification message)
        {
            var connectedUsers = connectionService.GetConnectedUsers();
            var connectionIds = message.RecipientIds
                .Where(connectedUsers.ContainsKey)
                .SelectMany(id => connectedUsers[id])
                .ToArray();
            await hubContext.Clients.Clients(connectionIds).Send(new
            {
                id = message.NotificationId,
                eventType = message.EventType.ToString(),
                data = message.Metadata
            });
            return ProcessResult.Success;
        }
    }
}