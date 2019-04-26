using System;
using DM.Services.Forum.Dto.Output;

namespace DM.Services.Forum.Dto.Internal
{
    /// <summary>
    /// DTO to remove the commentary
    /// </summary>
    public class CommentToDelete : Comment
    {
        /// <summary>
        /// Tells if the comment is last comment of its parent topic
        /// </summary>
        public bool IsLastCommentOfTopic { get; set; }
        
        /// <summary>
        /// Parent topic identifier
        /// </summary>
        public Guid TopicId { get; set; }
    }
}