using System;
using System.Collections.Generic;
using DM.Services.Core.Dto;

namespace DM.Services.Common.Dto
{
    public class Comment
    {
        public Guid Id { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? LastUpdateDate { get; set; }

        public string Text { get; set; }

        public GeneralUser Author { get; set; }
        public IEnumerable<GeneralUser> Likes { get; set; }
    }
}