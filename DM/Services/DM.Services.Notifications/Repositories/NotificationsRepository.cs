using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Services.Core.Dto;
using DM.Services.DataAccess.BusinessObjects.Notifications;
using DM.Services.DataAccess.MongoIntegration;
using MongoDB.Driver;

namespace DM.Services.Notifications.Repositories
{
    /// <inheritdoc cref="INotificationsRepository" />
    public class NotificationsRepository : MongoCollectionRepository<Notification>, INotificationsRepository
    {
        /// <inheritdoc />
        public NotificationsRepository(DmMongoClient client) : base(client)
        {
        }

        private static FilterDefinition<Notification> UserInterested(Guid userId) =>
            Filter.AnyEq(n => n.UsersInterested, userId);

        private static FilterDefinition<Notification> UserToBeNotified(Guid userId) =>
            UserInterested(userId) &
            !Filter.AnyEq(n => n.UsersNotified, userId);

        /// <inheritdoc />
        public Task<long> CountUnread(Guid userId) => Collection.CountDocumentsAsync(UserToBeNotified(userId));

        /// <inheritdoc />
        public async Task<IEnumerable<Notification>> GetNotifications(Guid userId, PagingData pagingData) =>
            await Collection
                .Find(UserInterested(userId))
                .Sort(Sort.Descending(n => n.CreateDate))
                .Skip(pagingData.Skip)
                .Limit(pagingData.Take)
                .ToListAsync();

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
}