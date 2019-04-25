using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Services.Authentication.Implementation;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Web.Core.Authentication.Credentials;
using Microsoft.AspNetCore.Http;

namespace DM.Web.Core.Authentication
{
    /// <inheritdoc />
    public class WebAuthenticationService : IWebAuthenticationService
    {
        private readonly IAuthenticationService authenticationService;
        private readonly ICredentialsStorage credentialsStorage;
        private readonly IIdentitySetter identitySetter;

        /// <inheritdoc />
        public WebAuthenticationService(
            IAuthenticationService authenticationService,
            ICredentialsStorage credentialsStorage,
            IIdentitySetter identitySetter)
        {
            this.authenticationService = authenticationService;
            this.credentialsStorage = credentialsStorage;
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
                case UnconditionalCredentials unconditionalCredentials:
                    return await authenticationService.Authenticate(unconditionalCredentials.UserId);
                default:
                    return Identity.Guest();
            }
        }

        /// <inheritdoc />
        public async Task<IIdentity> Authenticate(AuthCredentials credentials, HttpContext httpContext)
        {
            var identity = identitySetter.Current = await GetAuthenticationResult(credentials);
            await TryLoadAuthenticationResult(httpContext, identity);
            return identity;
        }

        /// <inheritdoc />
        public async Task Logout(HttpContext httpContext)
        {
            var identity = identitySetter.Current = await authenticationService.Logout();
            await TryLoadAuthenticationResult(httpContext, identity);
        }

        /// <inheritdoc />
        public async Task<IIdentity> LogoutAll(HttpContext httpContext)
        {
            var identity = identitySetter.Current = await authenticationService.LogoutAll();
            await TryLoadAuthenticationResult(httpContext, identity);
            return identity;
        }

        private Task TryLoadAuthenticationResult(HttpContext httpContext, IIdentity identity)
        {
            return identity.Error == AuthenticationError.NoError && identity.User.IsAuthenticated
                ? credentialsStorage.Load(httpContext, identity)
                : credentialsStorage.Unload(httpContext);
        }
    }
}