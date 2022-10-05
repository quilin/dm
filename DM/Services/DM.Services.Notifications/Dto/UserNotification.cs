using System;
using DM.Services.Core.Dto.Enums;

namespace DM.Services.Notifications.Dto;

/// <summary>
/// DTO model of received notification
/// </summary>
public class UserNotification
{
    /// <summary>
    /// Notification identifier
    /// </summary>
    public Guid NotificationId { get; set; }

    /// <summary>
    /// Event type
    /// </summary>
    public EventType EventType { get; set; }

    /// <summary>
    /// Creation moment
    /// </summary>
    public DateTimeOffset CreateDate { get; set; }

    /// <summary>
    /// Notification metadata
    /// </summary>
    public object Metadata { get; set; }
}