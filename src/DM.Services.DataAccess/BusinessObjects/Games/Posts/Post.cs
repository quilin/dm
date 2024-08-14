using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DM.Services.DataAccess.BusinessObjects.Common;
using DM.Services.DataAccess.BusinessObjects.DataContracts;
using DM.Services.DataAccess.BusinessObjects.Games.Characters;
using DM.Services.DataAccess.BusinessObjects.Games.Rating;
using DM.Services.DataAccess.BusinessObjects.Users;

namespace DM.Services.DataAccess.BusinessObjects.Games.Posts;

/// <summary>
/// DAL model for game post
/// </summary>
[Table("Posts")]
public class Post : IRemovable
{
    /// <summary>
    /// Post identifier
    /// </summary>
    [Key]
    public Guid PostId { get; set; }

    /// <summary>
    /// Room identifier
    /// </summary>
    public Guid RoomId { get; set; }

    /// <summary>
    /// Character identifier
    /// </summary>
    public Guid? CharacterId { get; set; }

    /// <summary>
    /// Author identifier
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Creation moment
    /// </summary>
    public DateTimeOffset CreateDate { get; set; }

    /// <summary>
    /// Last update author identifier
    /// </summary>
    public Guid? LastUpdateUserId { get; set; }

    /// <summary>
    /// Last update moment
    /// </summary>
    public DateTimeOffset? LastUpdateDate { get; set; }

    /// <summary>
    /// Post text
    /// </summary>
    public string Text { get; set; }

    /// <summary>
    /// Additional text
    /// </summary>
    public string Commentary { get; set; }

    /// <summary>
    /// Private message to master
    /// </summary>
    public string MasterMessage { get; set; }

    /// <inheritdoc />
    public bool IsRemoved { get; set; }

    /// <summary>
    /// Room
    /// </summary>
    [ForeignKey(nameof(RoomId))]
    public virtual Room Room { get; set; }

    /// <summary>
    /// Character
    /// </summary>
    [ForeignKey(nameof(CharacterId))]
    public virtual Character Character { get; set; }

    /// <summary>
    /// Author
    /// </summary>
    [ForeignKey(nameof(UserId))]
    public virtual User Author { get; set; }

    /// <summary>
    /// Last update author
    /// </summary>
    [ForeignKey(nameof(LastUpdateUserId))]
    public virtual User LastUpdateAuthor { get; set; }

    /// <summary>
    /// Votes for the post
    /// </summary>
    [InverseProperty(nameof(Vote.Post))]
    public virtual ICollection<Vote> Votes { get; set; }

    /// <summary>
    /// Files attached to post
    /// </summary>
    [InverseProperty(nameof(Upload.Post))]
    public virtual ICollection<Upload> Attachments { get; set; }
}