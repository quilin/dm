using System.Linq;
using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Services.Authentication.Extensions;
using DM.Services.Authentication.Implementation;
using DM.Web.Core.Authentication.Credentials;
using Microsoft.AspNetCore.Http;

namespace DM.Web.Core.Authentication
{
    public class WebAuthenticationService : IWebAuthenticationService
    {
        private readonly ICredentialsExtractor credentialsExtractor;
        private readonly IAuthenticationService authenticationService;
        private readonly IUserSetter userSetter;
        private readonly IAuthenticationStorage authenticationStorage;

        public WebAuthenticationService(
            ICredentialsExtractor credentialsExtractor,
            IAuthenticationService authenticationService,
            IUserSetter userSetter,
            IAuthenticationStorage authenticationStorage)
        {
            this.credentialsExtractor = credentialsExtractor;
            this.authenticationService = authenticationService;
            this.userSetter = userSetter;
            this.authenticationStorage = authenticationStorage;
        }

        public async Task Authenticate(HttpContext httpContext)
        {
            var authResult = await TryAuthenticate(httpContext);

            var bans = authResult.User.IsGuest()
                ? Enumerable.Empty<IntentionBan>()
                : new IntentionBan[0]; // todo: call moderation service

            userSetter.Current = new IntendingUser(authResult.User, bans);
            userSetter.CurrentSession = authResult.Session;

            await authenticationStorage.Store(authResult);
        }

        private async Task<AuthenticationResult> TryAuthenticate(HttpContext httpContext)
        {
            var (success, credentials) = await credentialsExtractor.Extract(httpContext);
            if (!success)
            {
                return AuthenticationResult.Success(AuthenticatingUser.Guest, null, null);
            }

            switch (credentials)
            {
                case LoginCredentials loginCredentials:
                    return await authenticationService.Authenticate(
                        loginCredentials.Login, loginCredentials.Password, loginCredentials.RememberMe);
                case TokenCredentials tokenCredentials:
                    return await authenticationService.Authenticate(tokenCredentials.Token);
                default:
                    return AuthenticationResult.Fail(AuthenticationError.SessionExpired);
            }
        }
    }
}