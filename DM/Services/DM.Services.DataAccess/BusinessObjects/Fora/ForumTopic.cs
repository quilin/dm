using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DM.Services.DataAccess.BusinessObjects.Common;
using DM.Services.DataAccess.BusinessObjects.DataContracts;
using DM.Services.DataAccess.BusinessObjects.Users;

namespace DM.Services.DataAccess.BusinessObjects.Fora;

/// <summary>
/// DAL model for forum topic
/// </summary>
[Table("ForumTopics")]
public class ForumTopic : IRemovable
{
    /// <summary>
    /// Topic identifier
    /// </summary>
    [Key]
    public Guid ForumTopicId { get; set; }

    /// <summary>
    /// Forum identifier
    /// </summary>
    public Guid ForumId { get; set; }

    /// <summary>
    /// Author identifier
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Creation moment
    /// </summary>
    public DateTimeOffset CreateDate { get; set; }

    /// <summary>
    /// Title
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Text
    /// </summary>
    public string Text { get; set; }

    /// <summary>
    /// If true, the topic will always appear on top of the forum topics list
    /// </summary>
    public bool Attached { get; set; }

    /// <summary>
    /// Closed topics are available in read-only mode
    /// </summary>
    public bool Closed { get; set; }

    /// <summary>
    /// Last commentary identifier
    /// For query optimisation purposes
    /// </summary>
    public Guid? LastCommentId { get; set; }

    /// <inheritdoc />
    public bool IsRemoved { get; set; }

    /// <summary>
    /// Forum
    /// </summary>
    [ForeignKey(nameof(ForumId))]
    public virtual Forum Forum { get; set; }

    /// <summary>
    /// Author
    /// </summary>
    [ForeignKey(nameof(UserId))]
    public virtual User Author { get; set; }

    /// <summary>
    /// Last commentary
    /// </summary>
    [ForeignKey(nameof(LastCommentId))]
    public virtual Comment LastComment { get; set; }

    /// <summary>
    /// Commenaries
    /// </summary>
    [InverseProperty(nameof(Comment.Topic))]
    public virtual ICollection<Comment> Comments { get; set; }

    /// <summary>
    /// Likes
    /// </summary>
    [InverseProperty(nameof(Like.Topic))]
    public virtual ICollection<Like> Likes { get; set; }
}