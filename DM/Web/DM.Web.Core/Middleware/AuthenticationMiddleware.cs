using System.Threading.Tasks;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Web.Core.Authentication;
using Microsoft.AspNetCore.Http;

namespace DM.Web.Core.Middleware
{
    /// <summary>
    /// Middleware for user authentication
    /// </summary>
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate next;

        /// <inheritdoc />
        public AuthenticationMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        /// <summary>
        /// Before request
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="authenticationService"></param>
        /// <param name="identitySetter"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext httpContext,
            IWebAuthenticationService authenticationService,
            IIdentitySetter identitySetter)
        {
            await authenticationService.Authenticate(httpContext);
            identitySetter.Refresh();
            await next(httpContext);
        }
    }
}