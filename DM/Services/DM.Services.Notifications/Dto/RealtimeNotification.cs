using System;
using System.Collections.Generic;

namespace DM.Services.Notifications.Dto;

/// <summary>
/// DTO model for notification that is ready to be received 
/// </summary>
public class RealtimeNotification : UserNotification
{
    /// <summary>
    /// Identifiers of users who might be interested in this notification
    /// </summary>
    public IEnumerable<Guid> RecipientIds { get; set; }
}