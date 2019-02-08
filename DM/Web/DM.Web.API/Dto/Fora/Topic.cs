using System;
using DM.Web.API.Dto.Common;

namespace DM.Web.API.Dto.Fora
{
    public class Topic
    {
        public string Id { get; set; }
        public User Author { get; set; }
        public DateTime Created { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public bool Attached { get; set; }
        public bool Closed { get; set; }
        public LastTopicComment LastComment { get; set; }
        public int CommentsCount { get; set; }
        public int UnreadCommentsCount { get; set; }

        public Forum Forum { get; set; }
    }

    public class LastTopicComment
    {
        public DateTime Created { get; set; }
        public User Author { get; set; }
    }
}