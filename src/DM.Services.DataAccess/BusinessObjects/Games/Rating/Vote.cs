using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DM.Services.Core.Dto.Enums;
using DM.Services.DataAccess.BusinessObjects.Games.Posts;
using DM.Services.DataAccess.BusinessObjects.Users;

namespace DM.Services.DataAccess.BusinessObjects.Games.Rating;

/// <summary>
/// DAL model for post vote
/// </summary>
[Table("Votes")]
public class Vote
{
    /// <summary>
    /// Vote identifier
    /// </summary>
    [Key]
    public Guid VoteId { get; set; }

    /// <summary>
    /// Post identifier
    /// </summary>
    public Guid PostId { get; set; }

    /// <summary>
    /// Game identifier
    /// </summary>
    public Guid GameId { get; set; }

    /// <summary>
    /// Author identifier
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Post author identifier
    /// </summary>
    public Guid TargetUserId { get; set; }

    /// <summary>
    /// Creation moment
    /// </summary>
    public DateTimeOffset CreateDate { get; set; }

    /// <summary>
    /// Vote reason
    /// </summary>
    public VoteType Type { get; set; }

    /// <summary>
    /// Rating impact for storage
    /// </summary>
    public short SignValue { get; set; }

    /// <summary>
    /// Rating impact sign
    /// </summary>
    public VoteSign Sign => (VoteSign) SignValue;

    /// <summary>
    /// Post
    /// </summary>
    [ForeignKey(nameof(PostId))]
    public virtual Post Post { get; set; }

    /// <summary>
    /// Game
    /// </summary>
    [ForeignKey(nameof(GameId))]
    public virtual Game Game { get; set; }

    /// <summary>
    /// Author
    /// </summary>
    [ForeignKey(nameof(UserId))]
    public virtual User VotedUser { get; set; }

    /// <summary>
    /// Post author
    /// </summary>
    [ForeignKey(nameof(TargetUserId))]
    public virtual User TargetUser { get; set; }
}