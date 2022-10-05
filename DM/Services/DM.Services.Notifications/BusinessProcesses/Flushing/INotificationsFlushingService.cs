using System;
using System.Threading.Tasks;

namespace DM.Services.Notifications.BusinessProcesses.Flushing;

/// <summary>
/// Service for marking notifications as read
/// </summary>
public interface INotificationsFlushingService
{
    /// <summary>
    /// Mark single notification as read by user
    /// </summary>
    /// <param name="notificationId">Notification identifier</param>
    /// <returns></returns>
    Task MarkAsRead(Guid notificationId);

    /// <summary>
    /// Mark all user notification as read
    /// </summary>
    /// <returns></returns>
    Task MarkAllAsRead();
}