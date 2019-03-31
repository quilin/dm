using System;
using DM.Services.DataAccess.BusinessObjects.Users;

namespace DM.Services.Authentication.Dto
{
    /// <inheritdoc />
    public class Identity : IIdentity
    {
        private Identity()
        {
        }

        /// <inheritdoc />
        public AuthenticatedUser User { get; private set; }

        /// <inheritdoc />
        public Session Session { get; private set; }

        /// <inheritdoc />
        public UserSettings Settings { get; private set; }

        /// <inheritdoc />
        public AuthenticationError Error { get; private set; }

        /// <inheritdoc />
        public string AuthenticationToken { get; private set; }

        /// <inheritdoc />
        public Guid CorrelationToken { get; private set; }

        /// <summary>
        /// Creates identity for an unauthenticated user
        /// </summary>
        /// <param name="error">Authentication error</param>
        /// <returns>Unauthenticated user identity</returns>
        public static IIdentity Fail(AuthenticationError error) => new Identity
        {
            Error = error,
            User = AuthenticatedUser.Guest,
            Session = null,
            Settings = UserSettings.Default,
            AuthenticationToken = null
        };

        /// <summary>
        /// Creates identity for an authenticated user
        /// </summary>
        /// <param name="user">Authenticated user</param>
        /// <param name="session">User session</param>
        /// <param name="settings">User settings</param>
        /// <param name="token">Authentication token</param>
        /// <returns>Authenticated user identity</returns>
        public static IIdentity Success(
            AuthenticatedUser user, Session session, UserSettings settings, string token) => new Identity
        {
            Error = AuthenticationError.NoError,
            User = user,
            Session = session,
            Settings = settings,
            AuthenticationToken = token
        };

        /// <summary>
        /// Creates identity for the guest user
        /// </summary>
        /// <returns>Guest user identity</returns>
        public static IIdentity Guest() => new Identity
        {
            Error = AuthenticationError.NoError,
            User = AuthenticatedUser.Guest,
            Settings = UserSettings.Default,
            AuthenticationToken = null,
            Session = null
        };
    }
}