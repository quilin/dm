using DM.Services.DataAccess.BusinessObjects.Users;

namespace DM.Services.Authentication.Dto
{
    public class Identity : IIdentity
    {
        private Identity()
        {
        }

        public AuthenticatedUser User { get; private set; }
        public Session Session { get; private set; }
        public UserSettings Settings { get; private set; }

        public AuthenticationError Error { get; private set; }
        public string Token { get; private set; }

        public static IIdentity Fail(AuthenticationError error) => new Identity
        {
            Error = error,
            User = AuthenticatedUser.Guest,
            Session = null,
            Settings = UserSettings.Default,
            Token = null
        };

        public static IIdentity Success(
            AuthenticatedUser user, Session session, UserSettings settings, string token) => new Identity
        {
            Error = AuthenticationError.NoError,
            User = user,
            Session = session,
            Settings = settings,
            Token = token
        };

        public static IIdentity Guest() => new Identity
        {
            Error = AuthenticationError.NoError,
            User = AuthenticatedUser.Guest,
            Settings = UserSettings.Default,
            Token = null,
            Session = null
        };
    }
}