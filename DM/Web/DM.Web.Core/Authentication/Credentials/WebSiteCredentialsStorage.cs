using System;
using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using Microsoft.AspNetCore.Http;

namespace DM.Web.Core.Authentication.Credentials
{
    /// <summary>
    /// Cookie-based authentication token storage
    /// </summary>
    public class WebSiteCredentialsStorage : ICredentialsStorage
    {
        private const string HttpAuthorizationCookie = "__AUTH_cookie";

        /// <inheritdoc />
        public Task<TokenCredentials> ExtractToken(HttpContext httpContext)
        {
            return httpContext.Request.Cookies.TryGetValue(HttpAuthorizationCookie, out var authCookie)
                ? Task.FromResult(new TokenCredentials {Token = authCookie})
                : Task.FromResult<TokenCredentials>(null);
        }

        /// <inheritdoc />
        public Task Load(HttpContext httpContext, IIdentity identity)
        {
            httpContext.Response.Cookies.Append(HttpAuthorizationCookie, identity.AuthenticationToken,
                new CookieOptions
                {
                    HttpOnly = true,
                    IsEssential = true,
                    Secure = false,
                    Expires = identity.Session.IsPersistent
                        ? identity.Session.ExpirationDate
                        : (DateTimeOffset?) null
                });
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task Unload(HttpContext httpContext)
        {
            httpContext.Response.Cookies.Delete(HttpAuthorizationCookie);
            return Task.CompletedTask;
        }
    }
}