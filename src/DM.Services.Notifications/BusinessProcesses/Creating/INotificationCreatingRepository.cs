using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Services.DataAccess.BusinessObjects.Notifications;

namespace DM.Services.Notifications.BusinessProcesses.Creating;

/// <summary>
/// Notifications storage
/// </summary>
internal interface INotificationCreatingRepository
{
    /// <summary>
    /// Create new notification
    /// </summary>
    /// <param name="notifications">Notification DAL models</param>
    /// <returns></returns>
    Task Create(IEnumerable<Notification> notifications);
}