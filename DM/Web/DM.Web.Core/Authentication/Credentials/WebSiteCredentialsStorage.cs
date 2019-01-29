using System;
using System.Linq;
using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using Microsoft.AspNetCore.Http;

namespace DM.Web.Core.Authentication.Credentials
{
    public class WebSiteCredentialsStorage
    {
        private const string HttpAuthorizationCookie = "__AUTH_cookie";
        private const string LoginKey = "login";
        private const string PasswordKey = "password";
        private const string DoNotRememberKey = "doNotRemember";

        public async Task<(bool success, AuthCredentials credentials)> Extract(HttpContext httpContext)
        {
            if (httpContext.Request.Cookies.TryGetValue(HttpAuthorizationCookie, out var authCookie))
            {
                return (true, new TokenCredentials {Token = authCookie});
            }

            var form = await httpContext.Request.ReadFormAsync();
            if (form.TryGetValue(LoginKey, out var loginValues) &&
                form.TryGetValue(PasswordKey, out var passwordValues))
            {
                return (true, new LoginCredentials
                {
                    Login = loginValues.First(),
                    Password = passwordValues.First(),
                    RememberMe = form.TryGetValue(DoNotRememberKey, out var rememberValues) &&
                        rememberValues.First().Contains(false.ToString())
                });
            }

            return default;
        }

        public Task Load(HttpContext httpContext, AuthenticationResult authenticationResult)
        {
            httpContext.Response.Cookies.Append(HttpAuthorizationCookie, authenticationResult.Token,
                new CookieOptions
                {
                    HttpOnly = true,
                    IsEssential = true,
                    Secure = false,
                    Expires = authenticationResult.Session.IsPersistent
                        ? authenticationResult.Session.ExpirationDate
                        : (DateTimeOffset?) null
                });
            return Task.CompletedTask;
        }
    }
}