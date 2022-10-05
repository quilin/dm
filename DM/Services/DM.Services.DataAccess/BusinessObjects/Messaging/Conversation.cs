using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DM.Services.DataAccess.BusinessObjects.Messaging;

/// <summary>
/// DAL model for users conversation
/// </summary>
[Table("Conversations")]
public class Conversation
{
    /// <summary>
    /// Conversation identifier
    /// </summary>
    public Guid ConversationId { get; set; }

    /// <summary>
    /// Sign of a basic conversation between two users
    /// </summary>
    public bool Visavi { get; set; }

    /// <summary>
    /// Last message identifier
    /// </summary>
    public Guid? LastMessageId { get; set; }

    /// <summary>
    /// Links with conversation participants
    /// </summary>
    [InverseProperty(nameof(UserConversationLink.Conversation))]
    public virtual ICollection<UserConversationLink> UserLinks { get; set; }

    /// <summary>
    /// Messages
    /// </summary>
    [InverseProperty(nameof(Message.Conversation))]
    public virtual ICollection<Message> Messages { get; set; }

    /// <summary>
    /// Last message
    /// </summary>
    [ForeignKey(nameof(LastMessageId))]
    public virtual Message LastMessage { get; set; }
}