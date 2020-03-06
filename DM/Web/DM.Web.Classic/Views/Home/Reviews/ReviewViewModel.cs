using System;
using DM.Web.Classic.Views.Shared.User;

namespace DM.Web.Classic.Views.Home.Reviews
{
    public class ReviewViewModel
    {
        public Guid ReviewId { get; set; }
        public UserViewModel Author { get; set; }
        public string Text { get; set; }

        public bool IsRandom { get; set; }
        public bool CanEdit { get; set; }
        public bool CanApprove { get; set; }
        public bool CanRemove { get; set; }
    }
}