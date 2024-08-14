using System;

namespace DM.Services.Gaming.Dto.Output;

/// <summary>
/// DTO model for game tag
/// </summary>
public class GameTag
{
    /// <summary>
    /// Identifier
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Tag group title
    /// </summary>
    public string GroupTitle { get; set; }

    /// <summary>
    /// Tag title
    /// </summary>
    public string Title { get; set; }
}