using System;
using DM.Services.Forum.Dto;
using DM.Web.API.Dto.Users;
using DM.Web.Core.Helpers;

namespace DM.Web.API.Dto.Fora
{
    public class Topic
    {
        public Topic(TopicsListItem topic)
        {
            Id = topic.Id.EncodeToReadable(topic.Title);
            Author = new User(topic.Author);
            Created = topic.CreateDate;
            Title = topic.Title;
            Description = topic.Text;
            Attached = topic.Attached;
            Closed = topic.Closed;
            LastComment = new LastTopicComment
            {
                Author = new User(topic.LastComment.Author),
                Created = topic.LastComment.CreateDate
            };
            CommentsCount = topic.TotalCommentsCount;
            UnreadCommentsCount = topic.UnreadCommentsCount;
            Forum = new Forum(topic.Forum);
        }

        public string Id { get; set; }
        public User Author { get; }
        public DateTime Created { get; }

        public string Title { get; set; }
        public string Description { get; set; }
        public bool Attached { get; set; }
        public bool Closed { get; set; }
        public LastTopicComment LastComment { get; }
        public int CommentsCount { get; }
        public int UnreadCommentsCount { get; }

        public Forum Forum { get; }
    }

    public class LastTopicComment
    {
        public DateTime Created { get; set; }
        public User Author { get; set; }
    }
}