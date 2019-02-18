using System;
using DM.Services.Core.Dto;

namespace DM.Services.Forum.Dto
{
    public class TopicsListItem
    {
        public Guid Id { get; set; }
        public ForaListItem Forum { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public DateTime CreateDate { get; set; }

        public GeneralUser Author { get; set; }

        public int TotalCommentsCount { get; set; }
        public int UnreadCommentsCount { get; set; }

        public LastComment LastComment { get; set; }
        public bool Attached { get; set; }
        public bool Closed { get; set; }

        public DateTime LastActivityDate { get; set; }
    }

    public class LastComment
    {
        public GeneralUser Author { get; set; }
        public DateTime CreateDate { get; set; }
    }
}