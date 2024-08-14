using DM.Services.Common.Dto;

namespace DM.Services.Forum.Dto.Internal;

/// <summary>
/// DTO to remove the commentary
/// </summary>
internal class CommentToDelete : Comment
{
    /// <summary>
    /// Tells if the comment is last comment of its parent topic
    /// </summary>
    public bool IsLastCommentOfTopic { get; set; }
}