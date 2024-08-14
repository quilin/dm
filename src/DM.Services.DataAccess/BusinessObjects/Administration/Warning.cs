using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DM.Services.DataAccess.BusinessObjects.Common;
using DM.Services.DataAccess.BusinessObjects.DataContracts;
using DM.Services.DataAccess.BusinessObjects.Users;

namespace DM.Services.DataAccess.BusinessObjects.Administration;

/// <summary>
/// DAL model for user warning
/// </summary>
[Table("Warnings")]
public class Warning : IAdministrated
{
    /// <summary>
    /// Warning identifier
    /// </summary>
    [Key]
    public Guid WarningId { get; set; }

    /// <inheritdoc />
    public Guid UserId { get; set; }

    /// <inheritdoc />
    public Guid ModeratorId { get; set; }

    /// <summary>
    /// Warning causation entity identifier (e.g. topic, comment, etc.)
    /// </summary>
    public Guid EntityId { get; set; }

    /// <summary>
    /// Creation moment
    /// </summary>
    public DateTimeOffset CreateDate { get; set; }

    /// <summary>
    /// Moderation message
    /// </summary>
    public string Text { get; set; }

    /// <summary>
    /// Warning points based on the violation
    /// </summary>
    public int Points { get; set; }

    /// <inheritdoc />
    public bool IsRemoved { get; set; }

    /// <inheritdoc />
    [ForeignKey(nameof(UserId))]
    public virtual User User { get; set; }

    /// <inheritdoc />
    [ForeignKey(nameof(ModeratorId))]
    public virtual User Moderator { get; set; }

    /// <summary>
    /// Warning causation forum commentary
    /// </summary>
    [ForeignKey(nameof(EntityId))]
    public virtual Comment Comment { get; set; }

    /// <summary>
    /// Warning causation chat message
    /// </summary>
    [ForeignKey(nameof(EntityId))]
    public virtual ChatMessage ChatMessage { get; set; }
}