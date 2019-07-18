using System;
using DM.Web.Classic.Views.Shared.User;

namespace DM.Web.Classic.Views.Fora.Topic
{
    public class TopicViewModel
    {
        public Guid ForumTopicId { get; set; }
        public DateTimeOffset CreateDate { get; set; }

        public UserViewModel Author { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public int CommentsCount { get; set; }
        public int UnreadCommentsCount { get; set; }
        
        public DateTimeOffset? LastCommentDate { get; set; }
        public string LastCommentAuthor { get; set; }

        public bool Attached { get; set; }
        public bool Closed { get; set; }
    }
}