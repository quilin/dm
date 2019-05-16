using System.Threading.Tasks;
using DM.Services.DataAccess.BusinessObjects.Notifications;
using DM.Services.DataAccess.MongoIntegration;

namespace DM.Services.Notifications.Consumer
{
    /// <inheritdoc cref="INotificationRepository" />
    public class NotificationRepository : MongoCollectionRepository<Notification>, INotificationRepository
    {
        /// <inheritdoc />
        public NotificationRepository(DmMongoClient client) : base(client)
        {
        }

        /// <inheritdoc />
        public Task Create(Notification notification) => Collection.InsertOneAsync(notification);
    }
}