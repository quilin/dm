using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Services.Authentication.Implementation;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Web.Core.Authentication.Credentials;
using Microsoft.AspNetCore.Http;

namespace DM.Web.Core.Authentication
{
    public class WebAuthenticationService : IWebAuthenticationService
    {
        private readonly ICredentialsStorage credentialsStorage;
        private readonly IAuthenticationService authenticationService;
        private readonly IIdentitySetter identitySetter;

        public WebAuthenticationService(
            ICredentialsStorage credentialsStorage,
            IAuthenticationService authenticationService,
            IIdentitySetter identitySetter)
        {
            this.credentialsStorage = credentialsStorage;
            this.authenticationService = authenticationService;
            this.identitySetter = identitySetter;
        }

        private async Task<IIdentity> GetAuthenticationResult(AuthCredentials credentials)
        {
            switch (credentials)
            {
                case LoginCredentials loginCredentials:
                    return await authenticationService.Authenticate(
                        loginCredentials.Login, loginCredentials.Password, loginCredentials.RememberMe);
                case TokenCredentials tokenCredentials:
                    return await authenticationService.Authenticate(tokenCredentials.Token);
                default:
                    return Identity.Guest();
            }
        }

        private async Task StoreAuthentication(HttpContext httpContext, IIdentity identity)
        {
            if (identity.Error == AuthenticationError.NoError && identity.User.IsAuthenticated)
            {
                await credentialsStorage.Load(httpContext, identity);
            }

            identitySetter.Current = identity;
        }

        public async Task Authenticate(LoginCredentials credentials, HttpContext httpContext)
        {
            var authenticationResult = await GetAuthenticationResult(credentials);
            await StoreAuthentication(httpContext, authenticationResult);
        }

        public async Task Authenticate(HttpContext httpContext)
        {
            var tokenCredentials = await credentialsStorage.ExtractToken(httpContext);
            var authenticationResult = await GetAuthenticationResult(tokenCredentials);
            await StoreAuthentication(httpContext, authenticationResult);
        }

        public async Task Logout(HttpContext httpContext)
        {
            await authenticationService.Logout();
            await credentialsStorage.Unload(httpContext);
        }

        public async Task LogoutAll(HttpContext httpContext)
        {
            var authenticationResult = await authenticationService.LogoutAll();
            await StoreAuthentication(httpContext, authenticationResult);
        }
    }
}