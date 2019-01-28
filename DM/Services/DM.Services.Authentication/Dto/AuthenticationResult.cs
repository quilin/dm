using DM.Services.DataAccess.BusinessObjects.Users;

namespace DM.Services.Authentication.Dto
{
    public class AuthenticationResult
    {
        private AuthenticationResult() {}

        public AuthenticatedUser User { get; private set; }
        public Session Session { get; private set; }
        public AuthenticationError Error { get; private set; }
        public string Token { get; private set; }

        public static AuthenticationResult Fail(AuthenticationError error) => new AuthenticationResult
        {
            Error = error,
            User = AuthenticatedUser.Guest,
            Session = null,
            Token = null
        };

        public static AuthenticationResult Success(AuthenticatedUser user, Session session, string token) => new AuthenticationResult
        {
            Error = AuthenticationError.NoError,
            User = user,
            Session = session,
            Token = token
        };
    }
}