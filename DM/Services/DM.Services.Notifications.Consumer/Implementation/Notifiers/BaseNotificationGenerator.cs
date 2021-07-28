using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DM.Services.Core.Dto.Enums;
using DM.Services.DataAccess.BusinessObjects.Notifications;
using DM.Services.Notifications.Dto;

namespace DM.Services.Notifications.Consumer.Implementation.Notifiers
{
    /// <inheritdoc />
    public abstract class BaseNotificationGenerator : INotificationGenerator
    {
        /// <summary>
        /// Event type that generator can process
        /// </summary>
        protected abstract EventType EventType { get; }

        /// <inheritdoc />
        public bool CanResolve(EventType eventType) => eventType == EventType;

        /// <inheritdoc />
        public async Task<IEnumerable<(Notification notification, RealtimeNotification userNotification)>>
            Generate(Guid entityId)
        {
            var notifications = await GenerateNotifications(entityId);
            return notifications.Select(n => (n, new RealtimeNotification
            {
                NotificationId = n.NotificationId,
                RecipientIds = n.UsersInterested,
                Metadata = n.Metadata,
                EventType = EventType,
            }));
        }

        /// <summary>
        /// Generate DAL models of notifications to be stored
        /// </summary>
        /// <param name="entityId">Entity identifier</param>
        /// <returns></returns>
        protected abstract Task<IEnumerable<Notification>> GenerateNotifications(Guid entityId);
    }
}