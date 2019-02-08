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

        public Task Authenticate(LoginCredentials credentials, HttpContext httpContext) =>
            AuthenticateWithCredentials(credentials, httpContext);

        public async Task Authenticate(HttpContext httpContext) =>
            await AuthenticateWithCredentials(await credentialsStorage.ExtractToken(httpContext), httpContext);

        private async Task AuthenticateWithCredentials(AuthCredentials credentials, HttpContext httpContext)
        {
            AuthenticationResult authResult;
            switch (credentials)
            {
                case LoginCredentials loginCredentials:
                    authResult = await authenticationService.Authenticate(
                        loginCredentials.Login, loginCredentials.Password, loginCredentials.RememberMe);
                    break;
                case TokenCredentials tokenCredentials:
                    authResult = await authenticationService.Authenticate(tokenCredentials.Token);
                    break;
                default:
                    authResult = AuthenticationResult.Guest();
                    break;
            }

            if (authResult.Error == AuthenticationError.NoError && !authResult.User.IsGuest)
            {
                await credentialsStorage.Load(httpContext, authResult);
            }

            identitySetter.Current = authResult;
        }
    }
}