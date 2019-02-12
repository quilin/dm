using System;
using DM.Services.Core.Dto;

namespace DM.Services.DataAccess.BusinessObjects.Fora
{
    public class ForumTopicsListItem
    {
        public Guid Id { get; set; }
        public Guid ForumId { get; set; }
        public string ForumTitle { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public DateTime CreateDate { get; set; }
        public GeneralUser Author { get; set; }
        public bool Attached { get; set; }
        public bool Closed { get; set; }

        public int TotalCommentsCount { get; set; }
        public int UnreadCommentsCount { get; set; }

        public GeneralUser LastCommentAuthor { get; set; }
        public DateTime LastCommentDate { get; set; }
    }
}