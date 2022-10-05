using System;

namespace DM.Services.Common.Dto;

/// <summary>
/// DTO model for updating existing forum comment
/// </summary>
public class UpdateComment
{
    /// <summary>
    /// Comment identifier
    /// </summary>
    public Guid CommentId { get; set; }

    /// <summary>
    /// New comment text
    /// </summary>
    public string Text { get; set; }
}