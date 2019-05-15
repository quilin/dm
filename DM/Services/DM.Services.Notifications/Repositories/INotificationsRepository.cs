using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Services.Core.Dto;
using DM.Services.DataAccess.BusinessObjects.Notifications;

namespace DM.Services.Notifications.Repositories
{
    /// <summary>
    /// Notifications storage
    /// </summary>
    public interface INotificationsRepository
    {
        /// <summary>
        /// Counts unread notifications for user
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <returns>Unread notifications count</returns>
        Task<long> CountUnread(Guid userId);

        /// <summary>
        /// Gets paged notifications for user
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <param name="pagingData">Paging data</param>
        /// <returns>List of notifications ordered by invocation date</returns>
        Task<IEnumerable<Notification>> GetNotifications(Guid userId, PagingData pagingData);

        /// <summary>
        /// Create new notification
        /// </summary>
        /// <param name="notification">Notification DAL model</param>
        /// <returns></returns>
        Task Create(Notification notification);

        /// <summary>
        /// Mark single notification as read by user
        /// </summary>
        /// <param name="notificationId">Notification identifier</param>
        /// <param name="userId">User identifier</param>
        /// <returns></returns>
        Task MarkAsRead(Guid notificationId, Guid userId);

        /// <summary>
        /// Mark all user notification as read
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <returns></returns>
        Task MarkAllAsRead(Guid userId);
    }
}