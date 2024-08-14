using System;
using System.Collections.Generic;
using DM.Services.Core.Dto.Enums;
using DM.Services.DataAccess.MongoIntegration;
using MongoDB.Bson.Serialization.Attributes;

namespace DM.Services.DataAccess.BusinessObjects.Notifications;

/// <summary>
/// DAL model for real-time notifications
/// </summary>
[MongoCollectionName("RealtimeNotifications")]
public class Notification
{
    /// <summary>
    /// Notification identifier
    /// </summary>
    public Guid NotificationId { get; set; }

    /// <summary>
    /// Creation moment
    /// </summary>
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime CreateDate { get; set; }

    /// <summary>
    /// List of users who should receive the notification
    /// </summary>
    public IEnumerable<Guid> UsersInterested { get; set; }

    /// <summary>
    /// List of users who already received the notification
    /// </summary>
    public IEnumerable<Guid> UsersNotified { get; set; }

    /// <summary>
    /// Notification metadata
    /// </summary>
    public object Metadata { get; set; }

    /// <summary>
    /// Event type
    /// </summary>
    public EventType EventType { get; set; }
}