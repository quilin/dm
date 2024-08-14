using System;
using System.Threading.Tasks;
using DM.Services.DataAccess.MongoIntegration;

namespace DM.Services.Notifications.BusinessProcesses.Flushing;

/// <inheritdoc cref="INotificationsFlushingRepository" />
internal class NotificationsFlushingRepository : NotificationQueriesRepository, INotificationsFlushingRepository
{
    /// <inheritdoc />
    public NotificationsFlushingRepository(DmMongoClient client) : base(client)
    {
    }

    /// <inheritdoc />
    public Task MarkAsRead(Guid notificationId, Guid userId) =>
        Collection.UpdateOneAsync(
            Filter.Eq(n => n.NotificationId, notificationId) &
            UserToBeNotified(userId),
            Update.Push(n => n.UsersNotified, userId));

    /// <inheritdoc />
    public Task MarkAllAsRead(Guid userId) =>
        Collection.UpdateManyAsync(
            UserToBeNotified(userId),
            Update.Push(n => n.UsersNotified, userId));
}