using System;
using System.Threading.Tasks;

namespace DM.Services.Notifications.BusinessProcesses.Flushing;

/// <summary>
/// Notifications storage
/// </summary>
internal interface INotificationsFlushingRepository
{
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