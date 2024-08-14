using System;

namespace DM.Web.API.Dto.Games;

/// <summary>
/// API DTO model for game tag
/// </summary>
public class Tag
{
    /// <summary>
    /// Identifier
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Tag display name
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Tag category name
    /// </summary>
    public string Category { get; set; }
}