using System.Threading.Tasks;
using DM.Services.DataAccess.BusinessObjects.Notifications;

namespace DM.Services.Notifications.Consumer
{
    /// <summary>
    /// Notifications storage
    /// </summary>
    public interface INotificationRepository
    {
        /// <summary>
        /// Create new notification
        /// </summary>
        /// <param name="notification">Notification DAL model</param>
        /// <returns></returns>
        Task Create(Notification notification);
    }
}