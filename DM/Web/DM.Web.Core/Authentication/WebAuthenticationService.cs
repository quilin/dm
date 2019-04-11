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
        private readonly IIdentitySetter identitySetter;

        /// <inheritdoc />
        public WebAuthenticationService(
            IAuthenticationService authenticationService,
            IIdentitySetter identitySetter)
        {
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
                case UnconditionalCredentials unconditionalCredentials:
                    return await authenticationService.Authenticate(unconditionalCredentials.UserId);
                default:
                    return Identity.Guest();
            }
        }

        /// <inheritdoc />
        public async Task<IIdentity> Authenticate(AuthCredentials credentials, HttpContext httpContext) =>
            identitySetter.Current = await GetAuthenticationResult(credentials);

        /// <inheritdoc />
        public async Task Logout(HttpContext httpContext) =>
            identitySetter.Current = await authenticationService.Logout();

        /// <inheritdoc />
        public async Task<IIdentity> LogoutAll(HttpContext httpContext) =>
            identitySetter.Current = await authenticationService.LogoutAll();
    }
}