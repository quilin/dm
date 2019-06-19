using DM.Web.Classic.Views.Community.CommunityUser;

namespace DM.Web.Classic.Views.Community
{
    public class CommunityViewModel
    {
        public int CurrentPage { get; set; }
        public int TotalPagesCount { get; set; }
        public int PageSize { get; set; }
        public int EntityNumber { get; set; }

        public CommunityUserViewModel[] Administrators { get; set; }
        public CommunityUserViewModel[] Moderators { get; set; }
        public CommunityUserViewModel[] Users { get; set; }
    }
}