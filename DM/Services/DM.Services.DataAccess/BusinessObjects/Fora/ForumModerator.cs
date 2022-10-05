using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DM.Services.DataAccess.BusinessObjects.Users;

namespace DM.Services.DataAccess.BusinessObjects.Fora;

/// <summary>
/// DAL model for moderator
/// </summary>
[Table("ForumModerators")]
public class ForumModerator
{
    /// <summary>
    /// Moderator identifier
    /// </summary>
    [Key]
    public Guid ForumModeratorId { get; set; }

    /// <summary>
    /// Forum identifier
    /// </summary>
    public Guid ForumId { get; set; }

    /// <summary>
    /// User identifier
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Forum
    /// </summary>
    [ForeignKey(nameof(ForumId))]
    public virtual Forum Forum { get; set; }

    /// <summary>
    /// User
    /// </summary>
    [ForeignKey(nameof(UserId))]
    public virtual User User { get; set; }
}