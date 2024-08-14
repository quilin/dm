using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Services.DataAccess.BusinessObjects.Notifications;
using DM.Services.DataAccess.MongoIntegration;

namespace DM.Services.Notifications.BusinessProcesses.Creating;

/// <inheritdoc cref="INotificationCreatingRepository" />
internal class NotificationCreatingRepository : MongoCollectionRepository<Notification>, INotificationCreatingRepository
{
    /// <inheritdoc />
    public NotificationCreatingRepository(DmMongoClient client) : base(client)
    {
    }

    /// <inheritdoc />
    public Task Create(IEnumerable<Notification> notifications) => Collection.InsertManyAsync(notifications);
}