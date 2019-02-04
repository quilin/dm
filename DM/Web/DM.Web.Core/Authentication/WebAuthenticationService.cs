using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Services.Authentication.Implementation;
using DM.Web.Core.Authentication.Credentials;
using Microsoft.AspNetCore.Http;

namespace DM.Web.Core.Authentication
{
    public class WebAuthenticationService : IWebAuthenticationService
    {
        private readonly ICredentialsStorage credentialsStorage;
        private readonly IAuthenticationService authenticationService;
        private readonly IUserSetter userSetter;

        public WebAuthenticationService(
            ICredentialsStorage credentialsStorage,
            IAuthenticationService authenticationService,
            IUserSetter userSetter)
        {
            this.credentialsStorage = credentialsStorage;
            this.authenticationService = authenticationService;
            this.userSetter = userSetter;
        }

        public async Task Authenticate(HttpContext httpContext)
        {
            var authResult = await TryAuthenticate(httpContext);

            userSetter.Current = authResult.User;
            userSetter.CurrentSession = authResult.Session;
            userSetter.CurrentSettings = authResult.Settings;

            if (authResult.Error == AuthenticationError.NoError)
            {
                await credentialsStorage.Load(httpContext, authResult);
            }
        }

        private async Task<AuthenticationResult> TryAuthenticate(HttpContext httpContext)
        {
            var (success, credentials) = await credentialsStorage.Extract(httpContext);
            if (!success)
            {
                return AuthenticationResult.Success(AuthenticatedUser.Guest, null, UserSettings.Default, null);
            }

            switch (credentials)
            {
                case LoginCredentials loginCredentials:
                    return await authenticationService.Authenticate(
                        loginCredentials.Login, loginCredentials.Password, loginCredentials.RememberMe);
                case TokenCredentials tokenCredentials:
                    return await authenticationService.Authenticate(tokenCredentials.Token);
                default:
                    return AuthenticationResult.Success(AuthenticatedUser.Guest, null, UserSettings.Default, null);
            }
        }
    }
}