using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DM.Services.DataAccess.BusinessObjects.DataContracts;
using DM.Services.DataAccess.BusinessObjects.Users;

namespace DM.Services.DataAccess.BusinessObjects.Messaging;

/// <summary>
/// DAL model for conversation message
/// </summary>
[Table("Messages")]
public class Message : IRemovable
{
    /// <summary>
    /// Message identifier
    /// </summary>
    [Key]
    public Guid MessageId { get; set; }

    /// <summary>
    /// Author identifier
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Conversation identifier
    /// </summary>
    public Guid ConversationId { get; set; }

    /// <summary>
    /// Creation moment
    /// </summary>
    public DateTimeOffset CreateDate { get; set; }

    /// <summary>
    /// Text
    /// </summary>
    public string Text { get; set; }

    /// <inheritdoc />
    public bool IsRemoved { get; set; }

    /// <summary>
    /// Author
    /// </summary>
    [ForeignKey(nameof(UserId))]
    public virtual User Author { get; set; }

    /// <summary>
    /// Conversation
    /// </summary>
    [ForeignKey(nameof(ConversationId))]
    public virtual Conversation Conversation { get; set; }
}