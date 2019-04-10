using System;

namespace DM.Services.Forum.Dto.Input
{
    /// <summary>
    /// DTO model for new commentary in topic
    /// </summary>
    public class CreateComment
    {
        /// <summary>
        /// Topic identifier
        /// </summary>
        public Guid TopicId { get; set; }

        /// <summary>
        /// Commentary text
        /// </summary>
        public string Text { get; set; }
    }
}