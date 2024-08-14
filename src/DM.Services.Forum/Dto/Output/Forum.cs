using System;
using System.Collections.Generic;
using DM.Services.Core.Dto.Enums;

namespace DM.Services.Forum.Dto.Output;

/// <summary>
/// Forum DTO model
/// </summary>
public class Forum
{
    /// <summary>
    /// Forum identifier
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Forum title
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Create topic policy
    /// </summary>
    public ForumAccessPolicy CreateTopicPolicy { get; set; }

    /// <summary>
    /// View topic policy
    /// </summary>
    public ForumAccessPolicy ViewPolicy { get; set; }

    /// <summary>
    /// Moderator identifiers
    /// </summary>
    public IEnumerable<Guid> ModeratorIds { get; set; }

    /// <summary>
    /// Total number of forum topics that has unread commentaries
    /// </summary>
    public int UnreadTopicsCount { get; set; }
}