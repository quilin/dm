using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DM.Services.DataAccess.BusinessObjects.Games.Posts;
using DM.Services.DataAccess.BusinessObjects.Users;

namespace DM.Services.DataAccess.BusinessObjects.Games.Links;

/// <summary>
/// DAL model for post anticipation (when users await one another in same room)
/// </summary>
public class PendingPost
{
    /// <summary>
    /// Anticipation identifier
    /// </summary>
    [Key]
    public Guid PendingPostId { get; set; }

    /// <summary>
    /// Waiting user identifier
    /// </summary>
    public Guid AwaitingUserId { get; set; }

    /// <summary>
    /// User that should write a post identifier
    /// </summary>
    public Guid PendingUserId { get; set; }

    /// <summary>
    /// Room identifier
    /// </summary>
    public Guid RoomId { get; set; }

    /// <summary>
    /// Creation moment
    /// </summary>
    public DateTimeOffset CreateDate { get; set; }

    /// <summary>
    /// Waiting user
    /// </summary>
    [ForeignKey(nameof(AwaitingUserId))]
    public virtual User AwaitingUser { get; set; }

    /// <summary>
    /// User that should write a post
    /// </summary>
    [ForeignKey(nameof(PendingUserId))]
    public virtual User PendingUser { get; set; }

    /// <summary>
    /// Room
    /// </summary>
    [ForeignKey(nameof(RoomId))]
    public virtual Room Room { get; set; }
}