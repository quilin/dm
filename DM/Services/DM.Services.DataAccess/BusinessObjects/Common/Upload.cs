using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DM.Services.DataAccess.BusinessObjects.DataContracts;
using DM.Services.DataAccess.BusinessObjects.Games;
using DM.Services.DataAccess.BusinessObjects.Games.Characters;
using DM.Services.DataAccess.BusinessObjects.Games.Posts;
using DM.Services.DataAccess.BusinessObjects.Users;

namespace DM.Services.DataAccess.BusinessObjects.Common;

/// <summary>
/// DAL model for user uploaded content
/// </summary>
[Table("Uploads")]
public class Upload : IRemovable
{
    /// <summary>
    /// Upload identifier
    /// </summary>
    [Key]
    public Guid UploadId { get; set; }

    /// <summary>
    /// Creation moment
    /// </summary>
    public DateTimeOffset CreateDate { get; set; }

    /// <summary>
    /// Linked entity identifier
    /// </summary>
    public Guid? EntityId { get; set; }

    /// <summary>
    /// Owner identifier
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Flag of file source: original or modified
    /// </summary>
    public bool Original { get; set; }

    /// <summary>
    /// Path to download or view the file
    /// </summary>
    [MaxLength(200)]
    public string FilePath { get; set; }

    /// <summary>
    /// Display name for the file
    /// </summary>
    [MaxLength(100)]
    public string FileName { get; set; }

    /// <inheritdoc />
    public bool IsRemoved { get; set; }

    /// <summary>
    /// Owner
    /// </summary>
    [ForeignKey(nameof(UserId))]
    public virtual User Owner { get; set; }

    /// <summary>
    /// User profile (for user profile picture)
    /// </summary>
    [ForeignKey(nameof(EntityId))]
    public virtual User UserProfile { get; set; }

    /// <summary>
    /// Game (for game preview picture)
    /// </summary>
    [ForeignKey(nameof(EntityId))]
    public virtual Game Game { get; set; }

    /// <summary>
    /// Character (for character portrait)
    /// </summary>
    [ForeignKey(nameof(EntityId))]
    public virtual Character Character { get; set; }

    /// <summary>
    /// Post (for post attachment)
    /// </summary>
    [ForeignKey(nameof(EntityId))]
    public virtual Post Post { get; set; }
}