namespace DM.Web.Classic.Views.Shared.User
{
    public class UserViewModel
    {
        public string Login { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public string Location { get; set; }
        public UserRating Rating { get; set; }
        public string ProfilePictureUrl { get; set; }

        public bool IsOnline { get; set; }
        public bool IsAdministrator { get; set; }
        public bool IsModerator { get; set; }
    }
}