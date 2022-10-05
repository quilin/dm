using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Services.DataAccess.BusinessObjects.Notifications;
using DM.Services.Notifications.Dto;

namespace DM.Services.Notifications.BusinessProcesses.Creating;

/// <summary>
/// Service for notifications creating
/// </summary>
public interface INotificationCreatingService
{
    /// <summary>
    /// Create new notifications
    /// </summary>
    /// <param name="createNotifications">List of creating DTOs</param>
    /// <returns></returns>
    Task<IEnumerable<Notification>> Create(IEnumerable<CreateNotification> createNotifications);
}