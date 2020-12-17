using System.Threading.Tasks;

namespace DM.Web.API.Notifications
{
    /// <summary>
    /// Realtime notifications SignalR hub
    /// </summary>
    public interface INotificationHub
    {
        /// <summary>
        /// Send notification
        /// </summary>
        /// <param name="notification"></param>
        /// <returns></returns>
        Task Send(object notification);
    }
}