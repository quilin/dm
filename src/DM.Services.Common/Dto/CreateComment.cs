using System;

namespace DM.Services.Common.Dto;

/// <summary>
/// DTO model for new commentary in topic
/// </summary>
public class CreateComment
{
    /// <summary>
    /// Topic identifier
    /// </summary>
    public Guid EntityId { get; set; }

    /// <summary>
    /// Commentary text
    /// </summary>
    public string Text { get; set; }
}