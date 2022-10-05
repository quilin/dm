using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DM.Services.DataAccess.BusinessObjects.Administration;
using DM.Services.DataAccess.BusinessObjects.Users;

namespace DM.Services.DataAccess.BusinessObjects.Common;

/// <summary>
/// DAL model for general chat message
/// </summary>
public class ChatMessage
{
    /// <summary>
    /// Identifier
    /// </summary>
    [Key]
    public Guid ChatMessageId { get; set; }

    /// <summary>
    /// Author identifier
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Creation moment in ticks
    /// </summary>
    public DateTimeOffset CreateDate { get; set; }

    /// <summary>
    /// Text
    /// </summary>
    public string Text { get; set; }

    /// <summary>
    /// Message author
    /// </summary>
    [ForeignKey(nameof(UserId))]
    public User Author { get; set; }

    /// <summary>
    /// Administrative warnings
    /// </summary>
    [InverseProperty(nameof(Warning.ChatMessage))]
    public virtual ICollection<Warning> Warnings { get; set; }
}