using System;
using System.Collections.Generic;
using DM.Services.DataAccess.MongoIntegration;

namespace DM.Services.DataAccess.BusinessObjects.Notifications
{
    /// <summary>
    /// DAL model for real-time notifications
    /// </summary>
    [MongoCollectionName("UnreadCounters")]
    public class Notification
    {
        /// <summary>
        /// Notification identifier
        /// </summary>
        public Guid NotificationId { get; set; }

        /// <summary>
        /// Creation moment
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// List of users who should receive the notification
        /// </summary>
        public IEnumerable<Guid> UsersInterested { get; set; }

        /// <summary>
        /// List of users who already received the notification
        /// </summary>
        public IEnumerable<Guid> UsersNotified { get; set; }

        /// <summary>
        /// Notification metadata
        /// </summary>
        public object Metadata { get; set; }
    }
}