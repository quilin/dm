using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Web.Core.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IFormAuthenticationService formAuthenticationService;
        private readonly ISecurityManager securityManager;
        private readonly IUserSetter userSetter;
        private readonly IUserSettingsService userSettingsService;
        private const string LoginKey = "login";
        private const string PasswordKey = "password";
        private const string DoNotRememberKey = "doNotRemember";

        public AuthenticationService(
            IFormAuthenticationService formAuthenticationService,
            ISecurityManager securityManager,
            IUserSetter userSetter,
            IUserSettingsService userSettingsService)
        {
            this.formAuthenticationService = formAuthenticationService;
            this.securityManager = securityManager;
            this.userSetter = userSetter;
            this.userSettingsService = userSettingsService;
        }

        public async Task<AuthenticationResult> Authenticate(HttpContext httpContext)
        {
            User user;

            var loginFetchResult = await TryGetLoginAndPassword(httpContext);
            if (!loginFetchResult.Success)
            {
                return formAuthenticationService.GetUserSession(httpContext, out user, out var oldSession)
                    ? new AuthenticationResult
                        {User = user, Session = oldSession, Error = UserAuthenticationError.NoError}
                    : new AuthenticationResult
                        {User = user, Session = oldSession, Error = UserAuthenticationError.SessionExpired};
            }

            var error = securityManager.Authenticate(loginFetchResult.Login, loginFetchResult.Password, out user);
            if (error != UserAuthenticationError.NoError)
            {
                return new AuthenticationResult{User = user, Error = error};
            }
                
            formAuthenticationService.LogIn(httpContext, user.UserId, loginFetchResult.Remember, out var session);
            return new AuthenticationResult{User = user, Session = session, Error = error};
        }

        public void SetUserCredentials(AuthenticationResult authenticationResult)
        {
            userSetter.Current = IntendingUser.FromUser(authenticationResult.User);
            userSetter.CurrentSession = authenticationResult.Session;
            userSetter.CurrentAuthenticationState = authenticationResult.Error;
            userSetter.CurrentSettings = authenticationResult.User.IsGuest()
                ? UserSettings.Default
                : userSettingsService.Find(authenticationResult.User.UserId) ?? UserSettings.Default;
        }

        private static async Task<LoginAndPasswordFetchResult> TryGetLoginAndPassword(HttpContext httpContext)
        {
            try
            {
                var form = await httpContext.Request.ReadFormAsync();
                if (form.TryGetValue(LoginKey, out var loginValues) &&
                    form.TryGetValue(PasswordKey, out var passwordValues))
                {
                    return new LoginAndPasswordFetchResult
                    {
                        Success = true,
                        Login = loginValues.First(),
                        Password = passwordValues.First(),
                        Remember = form.TryGetValue(DoNotRememberKey, out var rememberValues) &&
                                   rememberValues.First().Contains("false")
                    };
                }

                return new LoginAndPasswordFetchResult();
            }
            catch (InvalidOperationException)
            {
                return new LoginAndPasswordFetchResult();
            }
        }
        
        private class LoginAndPasswordFetchResult
        {
            public bool Success { get; set; }
            public string Login { get; set; }
            public string Password { get; set; }
            public bool Remember { get; set; }
        }
    }
}