using System.Threading.Tasks;
using DM.Web.Core.Authentication;
using Microsoft.AspNetCore.Http;

namespace DM.Web.Core.Middleware
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate next;

        public AuthenticationMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext, IWebAuthenticationService authenticationService)
        {
            // todo: only allow login credentials authentication for the certain endpoint, such as '/account/'
            await authenticationService.Authenticate(httpContext);
            await next(httpContext);
        }
    }
}