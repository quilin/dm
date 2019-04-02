using System.Threading.Tasks;
using DM.Services.Core.Dto.Enums;
using DM.Web.Core.Authentication;
using Microsoft.AspNetCore.Http;
using Serilog.Context;

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
            var identity = await authenticationService.Authenticate(httpContext);
            LogContext.PushProperty("User", identity.User.Role == UserRole.Guest ? "Guest" : identity.User.Login);
            await next(httpContext);
        }
    }
}