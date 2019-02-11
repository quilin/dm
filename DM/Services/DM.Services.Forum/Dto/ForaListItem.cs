using System;
using System.Collections.Generic;
using DM.Services.Core.Dto;

namespace DM.Services.Forum.Dto
{
    public class ForaListItem
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public IEnumerable<GeneralUser> Moderators { get; set; }
        public int UnreadTopicsCount { get; set; }
    }
}