using System;
using DM.Services.DataAccess.BusinessObjects.Notifications;
using DM.Services.DataAccess.MongoIntegration;
using MongoDB.Driver;

namespace DM.Services.Notifications.BusinessProcesses;

/// <inheritdoc />
public abstract class NotificationQueriesRepository : MongoCollectionRepository<Notification>
{
    /// <inheritdoc />
    protected NotificationQueriesRepository(DmMongoClient client) : base(client)
    {
    }
        
    /// <summary>
    /// Filter out only notifications that given user is interested with
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    protected static FilterDefinition<Notification> UserInterested(Guid userId) =>
        Filter.AnyEq(n => n.UsersInterested, userId);

    /// <summary>
    /// Filter out only notifications that given user is interested with but haven't yet read
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    protected static FilterDefinition<Notification> UserToBeNotified(Guid userId) =>
        UserInterested(userId) &
        !Filter.AnyEq(n => n.UsersNotified, userId);
}