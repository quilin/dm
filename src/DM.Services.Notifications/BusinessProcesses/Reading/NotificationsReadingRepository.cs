using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Services.Core.Dto;
using DM.Services.DataAccess.MongoIntegration;
using DM.Services.Notifications.Dto;
using MongoDB.Driver;

namespace DM.Services.Notifications.BusinessProcesses.Reading;

/// <inheritdoc cref="INotificationsReadingRepository" />
internal class NotificationsReadingRepository : NotificationQueriesRepository, INotificationsReadingRepository
{
    /// <inheritdoc />
    public NotificationsReadingRepository(DmMongoClient client) : base(client)
    {
    }

    /// <inheritdoc />
    public Task<long> Count(Guid userId) => Collection.CountDocumentsAsync(UserInterested(userId));

    /// <inheritdoc />
    public Task<long> CountUnread(Guid userId) => Collection.CountDocumentsAsync(UserToBeNotified(userId));

    /// <inheritdoc />
    public async Task<IEnumerable<UserNotification>> GetNotifications(Guid userId, PagingData pagingData)
    {
        return await Collection
            .Find(UserInterested(userId))
            .Sort(Sort.Descending(n => n.CreateDate))
            .Skip(pagingData.Skip)
            .Limit(pagingData.Take)
            .Project(Project.As<UserNotification>())
            .ToListAsync();
    }
}