using System;
using DM.Services.Forum.Dto;

namespace DM.Web.API.Dto.Fora
{
    public class Topic
    {
        public Topic(TopicsListItem topicsListItem)
        {
            Id = topicsListItem.Id;
            ForumId = topicsListItem.ForumId;
            Title = topicsListItem.Title;
            Text = topicsListItem.Text;
            Author = topicsListItem.Author;
            CreateDate = topicsListItem.CreateDate;
            TotalCommentsCount = topicsListItem.TotalCommentsCount;
            UnreadCommentsCount = topicsListItem.UnreadCommentsCount;
            LastComment = new TopicComment(topicsListItem);
        }

        public Guid Id { get; }
        public string ForumId { get; }
        public string Title { get; }
        public string Text { get; }
        public string Author { get; }
        public DateTime CreateDate { get; }

        public int TotalCommentsCount { get; }
        public int UnreadCommentsCount { get; }

        public TopicComment LastComment { get; }
    }

    public class TopicComment
    {
        public TopicComment(TopicsListItem topicsListItem)
        {
            Author = topicsListItem.LastCommentAuthor;
            CreateDate = topicsListItem.LastCommentDate;
        }

        public string Author { get; }
        public DateTime CreateDate { get; }
    }
}