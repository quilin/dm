using System;
using System.Collections.Generic;
using DM.Services.Core.Implementation;
using DM.Services.DataAccess.BusinessObjects.Notifications;
using DM.Services.Notifications.Dto;

namespace DM.Services.Notifications.BusinessProcesses.Creating;

/// <inheritdoc />
internal class NotificationFactory : INotificationFactory
{
    private readonly IGuidFactory guidFactory;

    /// <inheritdoc />
    public NotificationFactory(
        IGuidFactory guidFactory)
    {
        this.guidFactory = guidFactory;
    }

    /// <inheritdoc />
    public Notification Create(CreateNotification createNotification, DateTimeOffset createDate) => new()
    {
        NotificationId = guidFactory.Create(),
        CreateDate = createDate.UtcDateTime,
        EventType = createNotification.EventType,
        UsersNotified = new List<Guid>(),
        UsersInterested = createNotification.UsersInterested,
        Metadata = createNotification.Metadata
    };
}