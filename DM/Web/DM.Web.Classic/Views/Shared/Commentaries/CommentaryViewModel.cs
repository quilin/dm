using System;
using DM.Web.Classic.Views.Shared.User;

namespace DM.Web.Classic.Views.Shared.Commentaries
{
    public class CommentaryViewModel
    {
        public Guid CommentaryId { get; set; }
        public Guid EntityId { get; set; }
        public DateTimeOffset CreateDate { get; set; }
        public DateTimeOffset? LastUpdateDate { get; set; }
        public UserViewModel Author { get; set; }
        public string Text { get; set; }
        public int LikesCount { get; set; }

        public bool CanEdit { get; set; }
        public bool CanRemove { get; set; }
        public bool CanWarn { get; set; }

        public bool CanLike { get; set; }
        public bool HasLiked { get; set; }
    }
}