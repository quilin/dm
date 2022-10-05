using System;
using DM.Services.Core.Dto.Enums;

namespace DM.Services.Search.Dto;

/// <summary>
/// DTO model for search result
/// </summary>
public class FoundEntity
{
    /// <summary>
    /// Entity identifier
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Nature of found entity
    /// </summary>
    public SearchEntityType Type { get; set; }

    /// <summary>
    /// Title with highlights
    /// </summary>
    public string FoundTitle { get; set; }

    /// <summary>
    /// Original title
    /// </summary>
    public string OriginalTitle { get; set; }

    /// <summary>
    /// Text with highlights
    /// </summary>
    public string FoundText { get; set; }
}