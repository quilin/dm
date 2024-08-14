using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DM.Services.Core.Dto.Enums;

namespace DM.Services.DataAccess.BusinessObjects.Fora;

/// <summary>
/// DAL model for forum
/// </summary>
[Table("Fora")]
public class Forum
{
    /// <summary>
    /// Forum identifier
    /// </summary>
    [Key]
    public Guid ForumId { get; set; }

    /// <summary>
    /// Forum title
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Short description
    /// </summary>
    [Obsolete("Not used on website")]
    public string Description { get; set; }

    /// <summary>
    /// Display order number
    /// </summary>
    public int Order { get; set; }

    /// <summary>
    /// Role restrictions to view the forum content and the forum itself
    /// </summary>
    public ForumAccessPolicy ViewPolicy { get; set; }

    /// <summary>
    /// Role restrictions to create topics within the forum
    /// </summary>
    public ForumAccessPolicy CreateTopicPolicy { get; set; }

    /// <summary>
    /// Forum moderators
    /// </summary>
    [InverseProperty(nameof(ForumModerator.Forum))]
    public virtual ICollection<ForumModerator> Moderators { get; set; }

    /// <summary>
    /// Forum topics
    /// </summary>
    [InverseProperty(nameof(ForumTopic.Forum))]
    public virtual ICollection<ForumTopic> Topics { get; set; }
}