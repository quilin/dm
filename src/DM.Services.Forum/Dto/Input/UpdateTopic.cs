using System;

namespace DM.Services.Forum.Dto.Input;

/// <summary>
/// DTO model for updating existing topic
/// </summary>
public class UpdateTopic
{
    /// <summary>
    /// Topic identifier
    /// </summary>
    public Guid TopicId { get; set; }

    /// <summary>
    /// New title
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// New description
    /// </summary>
    public string Text { get; set; }

    /// <summary>
    /// New parent forum title
    /// </summary>
    public string ForumTitle { get; set; }

    /// <summary>
    /// Is attached
    /// </summary>
    public bool? Attached { get; set; }

    /// <summary>
    /// Is closed
    /// </summary>
    public bool? Closed { get; set; }
}