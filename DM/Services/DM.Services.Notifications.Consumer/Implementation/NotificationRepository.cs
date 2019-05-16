using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Services.DataAccess.BusinessObjects.Notifications;
using DM.Services.DataAccess.MongoIntegration;

namespace DM.Services.Notifications.Consumer.Implementation
{
    /// <inheritdoc cref="INotificationRepository" />
    public class NotificationRepository : MongoCollectionRepository<Notification>, INotificationRepository
    {
        /// <inheritdoc />
        public NotificationRepository(DmMongoClient client) : base(client)
        {
        }

        /// <inheritdoc />
        public Task Create(IEnumerable<Notification> notifications) => Collection.InsertManyAsync(notifications);
    }
}