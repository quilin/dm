using System;

namespace DM.Services.Notifications.Dto
{
    /// <summary>
    /// DTO model of received notification
    /// </summary>
    public class UserNotification
    {
        /// <summary>
        /// Notification identifier
        /// </summary>
        public Guid NotificationId { get; set; }

        /// <summary>
        /// Creation moment
        /// </summary>
        public DateTimeOffset CreateDate { get; set; }

        /// <summary>
        /// Notification metadata
        /// </summary>
        public object Metadata { get; set; }
    }
}