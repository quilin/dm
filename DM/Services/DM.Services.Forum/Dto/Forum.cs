using System;
using System.Collections.Generic;
using DM.Services.Core.Dto.Enums;

namespace DM.Services.Forum.Dto
{
    /// <summary>
    /// Forum DTO model
    /// </summary>
    public class Forum
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public ForumAccessPolicy CreateTopicPolicy { get; set; }
        public IEnumerable<Guid> ModeratorIds { get; set; }
        public int UnreadTopicsCount { get; set; }
    }
}