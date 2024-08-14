using System;
using System.ComponentModel.DataAnnotations.Schema;
using DM.Services.DataAccess.BusinessObjects.DataContracts;
using DM.Services.DataAccess.BusinessObjects.Users;

namespace DM.Services.DataAccess.BusinessObjects.Messaging;

/// <summary>
/// DAL model for conversation participant
/// </summary>
[Table("UserConversationLinks")]
public class UserConversationLink : IRemovable
{
    /// <summary>
    /// Link identifier
    /// </summary>
    public Guid UserConversationLinkId { get; set; }

    /// <summary>
    /// Participant identifier
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Conversation identifier
    /// </summary>
    public Guid ConversationId { get; set; }

    /// <inheritdoc />
    public bool IsRemoved { get; set; }

    /// <summary>
    /// Participant
    /// </summary>
    [ForeignKey(nameof(UserId))]
    public virtual User User { get; set; }

    /// <summary>
    /// Conversation
    /// </summary>
    [ForeignKey(nameof(ConversationId))]
    public virtual Conversation Conversation { get; set; }
}