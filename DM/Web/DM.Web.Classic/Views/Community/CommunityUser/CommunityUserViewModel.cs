using System;
using DM.Web.Classic.Views.Shared.User;

namespace DM.Web.Classic.Views.Community.CommunityUser
{
    public class CommunityUserViewModel
    {
        public int Number { get; set; }
        public Guid UserId { get; set; }
        public UserViewModel User { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }

        public bool CanWriteMessage { get; set; }
        public bool CanLogIn { get; set; }
    }
}