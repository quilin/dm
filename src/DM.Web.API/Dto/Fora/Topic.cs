using System;
using System.Collections.Generic;
using DM.Web.API.BbRendering;
using DM.Web.API.Dto.Users;

namespace DM.Web.API.Dto.Fora;

/// <summary>
/// API DTO model for topic
/// </summary>
public class Topic
{
    /// <summary>
    /// Topic identifier
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Author
    /// </summary>
    public User Author { get; set; }

    /// <summary>
    /// Creation moment
    /// </summary>
    public DateTimeOffset Created { get; set; }

    /// <summary>
    /// Title
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Description
    /// </summary>
    public CommonBbText Description { get; set; }

    /// <summary>
    /// Attached
    /// </summary>
    public bool? Attached { get; set; }

    /// <summary>
    /// Closed
    /// </summary>
    public bool? Closed { get; set; }

    /// <summary>
    /// Last commentary
    /// </summary>
    public LastTopicComment LastComment { get; set; }

    /// <summary>
    /// Total commentaries count
    /// </summary>
    public int CommentsCount { get; set; }

    /// <summary>
    /// Number of unread commentaries
    /// </summary>
    public int UnreadCommentsCount { get; set; }

    /// <summary>
    /// Forum
    /// </summary>
    public Forum Forum { get; set; }

    /// <summary>
    /// Users who like this
    /// </summary>
    public IEnumerable<User> Likes { get; set; }
}

/// <summary>
/// Last topic commentary DTO
/// </summary>
public class LastTopicComment
{
    /// <summary>
    /// Creation moment
    /// </summary>
    public DateTimeOffset Created { get; set; }

    /// <summary>
    /// Author
    /// </summary>
    public User Author { get; set; }
}