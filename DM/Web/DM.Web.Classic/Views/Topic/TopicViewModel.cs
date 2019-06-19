using System;
using DM.Web.Classic.Views.Shared.Commentaries;
using DM.Web.Classic.Views.Shared.User;

namespace DM.Web.Classic.Views.Topic
{
    public class TopicViewModel
    {
        public Guid TopicId { get; set; }
        public Guid ForumId { get; set; }
        public string ForumTitle { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public UserViewModel Author { get; set; }
        public DateTime CreateDate { get; set; }

        public bool Attached { get; set; }
        public bool Closed { get; set; }

		public TopicActionsViewModel TopicActions { get; set; }
        public CommentariesViewModel Commentaries { get; set; }

        public int? NumberToHighLight { get; set; }
    }
}