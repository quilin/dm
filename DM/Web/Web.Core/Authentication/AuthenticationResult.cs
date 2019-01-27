using DM.Services.DataAccess.BusinessObjects.Users;

namespace Web.Core.Authentication
{
    public class AuthenticationResult
    {
        public User User { get; set; }
        public Session Session { get; set; }
        public UserAuthenticationError Error { get; set; }
    }
}