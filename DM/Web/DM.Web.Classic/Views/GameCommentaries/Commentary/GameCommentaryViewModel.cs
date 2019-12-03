using System;
using System.Collections.Generic;
using DM.Web.Classic.Views.Shared.User;

namespace DM.Web.Classic.Views.GameCommentaries.Commentary
{
    public class GameCommentaryViewModel
    {
        public Guid CommentaryId { get; set; }
        public UserViewModel Author { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public string Text { get; set; }
        public int LikesCount { get; set; }

        public IEnumerable<string> CharacterNames { get; set; }

        public bool CanEdit { get; set; }
        public bool CanRemove { get; set; }

        public bool CanLike { get; set; }
        public bool HasLiked { get; set; }

        public bool CanWarn { get; set; }
    }
}