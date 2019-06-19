using System;

namespace DM.Web.Classic.Views.Shared.ForaList
{
    public class ForaListItemViewModel
    {
        public Guid ForumId { get; set; }
        public string Title { get; set; }
        public int UnreadTopicsCount { get; set; }
        public bool IsCurrent { get; set; }
    }
}