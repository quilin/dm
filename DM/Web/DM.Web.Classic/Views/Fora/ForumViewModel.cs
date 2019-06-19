using System;
using DM.Web.Classic.Views.Fora.CreateTopic;
using DM.Web.Classic.Views.Fora.Topic;
using DM.Web.Classic.Views.Shared.User;

namespace DM.Web.Classic.Views.Fora
{
    public class ForumViewModel
    {
        public Guid ForumId { get; set; }
        public string Title { get; set; }
        public UserViewModel[] Moderators { get; set; }

        public int TotalPagesCount { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int EntityNumber { get; set; }

        public bool CanCreateTopic { get; set; }
        public bool CanMarkAsRead { get; set; }

        public TopicViewModel[] AttachedTopics { get; set; }
        public TopicViewModel[] Topics { get; set; }
        public CreateTopicForm CreateForm { get; set; }
    }
}