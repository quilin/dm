using System;
using System.Collections.Generic;
using DM.Services.Core.Dto;

namespace DM.Services.Common.Dto;

/// <summary>
/// DTO model for comment
/// </summary>
public class Comment : ILikable
{
    /// <inheritdoc />
    public Guid Id { get; set; }
        
    /// <summary>
    /// Parent entity identifier
    /// </summary>
    public Guid EntityId { get; set; }

    /// <summary>
    /// Date of creation
    /// </summary>
    public DateTimeOffset CreateDate { get; set; }

    /// <summary>
    /// Date of last update
    /// </summary>
    public DateTimeOffset? LastUpdateDate { get; set; }

    /// <summary>
    /// Text
    /// </summary>
    public string Text { get; set; }

    /// <summary>
    /// Author
    /// </summary>
    public GeneralUser Author { get; set; }

    /// <inheritdoc />
    public IEnumerable<GeneralUser> Likes { get; set; }
}