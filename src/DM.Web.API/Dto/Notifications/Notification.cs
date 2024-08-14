using System;
using DM.Services.Core.Dto.Enums;

namespace DM.Web.API.Dto.Notifications;

/// <summary>
/// DTO model for user notification
/// </summary>
public class Notification
{
    /// <summary>
    /// Notification identifier
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Event type
    /// </summary>
    public EventType EventType { get; set; }

    /// <summary>
    /// Notification payload
    /// </summary>
    public object Payload { get; set; }
}