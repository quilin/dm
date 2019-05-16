using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Services.Core.Dto.Enums;
using DM.Services.DataAccess.BusinessObjects.Notifications;
using DM.Services.Notifications.Dto;

namespace DM.Services.Notifications.Consumer.Implementation.Notifiers
{
    /// <summary>
    /// Generates notification for certain event
    /// </summary>
    public interface INotificationGenerator
    {
        /// <summary>
        /// Tells if resolver can process the certain event type
        /// </summary>
        /// <param name="eventType">Event type</param>
        /// <returns></returns>
        bool CanResolve(EventType eventType);

        /// <summary>
        /// Prepares notifications for users
        /// </summary>
        /// <param name="entityId">Entity identifier</param>
        /// <returns>List of notifications to invoke</returns>
        Task<IEnumerable<(Notification notification, RealtimeNotification userNotification)>> Generate(Guid entityId);
    }
}