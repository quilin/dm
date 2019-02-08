using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Services.Authentication.Implementation;
using DM.Web.Core.Authentication.Credentials;
using Microsoft.AspNetCore.Http;

namespace DM.Web.Core.Authentication
{
    public class WebAuthenticationService : IWebAuthenticationService
    {
        private readonly IEnumerable<ICredentialsExtractor> credentialsExtractors;
        private readonly ICredentialsLoader credentialsLoader;
        private readonly IAuthenticationService authenticationService;
        private readonly IUserSetter userSetter;

        public WebAuthenticationService(
            IEnumerable<ICredentialsExtractor> credentialsExtractors,
            ICredentialsLoader credentialsLoader,
            IAuthenticationService authenticationService,
            IUserSetter userSetter)
        {
            this.credentialsExtractors = credentialsExtractors;
            this.credentialsLoader = credentialsLoader;
            this.authenticationService = authenticationService;
            this.userSetter = userSetter;
        }

        public async Task Authenticate<TCredentials>(HttpContext httpContext) where TCredentials : AuthCredentials
        {
            var authResult = await TryAuthenticate<TCredentials>(httpContext);

            userSetter.Current = authResult.User;
            userSetter.CurrentSession = authResult.Session;
            userSetter.CurrentSettings = authResult.Settings;

            if (authResult.Error == AuthenticationError.NoError &&
                authResult.User.IsGuest)
            {
                await credentialsLoader.Load(httpContext, authResult);
            }
        }

        private async Task<AuthenticationResult> TryAuthenticate<TCredentials>(HttpContext httpContext)
            where TCredentials : AuthCredentials
        {
            var credentialsExtractor = credentialsExtractors.OfType<ICredentialsExtractor<TCredentials>>().First();
            var (success, credentials) = await credentialsExtractor.Extract(httpContext);
            if (!success)
            {
                return AuthenticationResult.Guest();
            }

            switch (credentials)
            {
                case LoginCredentials loginCredentials:
                    return await authenticationService.Authenticate(
                        loginCredentials.Login, loginCredentials.Password, loginCredentials.RememberMe);
                case TokenCredentials tokenCredentials:
                    return await authenticationService.Authenticate(tokenCredentials.Token);
                default:
                    return AuthenticationResult.Guest();
            }
        }
    }
}