using System;

namespace DM.Services.Community.BusinessProcesses.Messaging.Creating;

/// <summary>
/// DTO for creating message
/// </summary>
public class CreateMessage
{
    /// <summary>
    /// Conversation identifier
    /// </summary>
    public Guid ConversationId { get; set; }

    /// <summary>
    /// Message content
    /// </summary>
    public string Text { get; set; }
}