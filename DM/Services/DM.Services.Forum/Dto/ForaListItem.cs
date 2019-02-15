using System;

namespace DM.Services.Forum.Dto
{
    public class ForaListItem
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public int UnreadTopicsCount { get; set; }
    }
}