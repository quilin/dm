using System.Threading.Tasks;
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
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext httpContext,
            IWebAuthenticationService authenticationService)
        {
            await authenticationService.Authenticate(httpContext);
            await next(httpContext);
        }
    }
}