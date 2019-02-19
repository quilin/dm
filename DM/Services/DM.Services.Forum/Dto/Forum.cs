using System;
using DM.Services.Core.Dto.Enums;

namespace DM.Services.Forum.Dto
{
    public class Forum
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public ForumAccessPolicy CreateTopicPolicy { get; set; }
        public int UnreadTopicsCount { get; set; }
    }
}