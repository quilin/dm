using System.Threading.Tasks;
using DM.Web.Core.Authentication;
using DM.Web.Core.Authentication.Credentials;
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

        public async Task InvokeAsync(HttpContext httpContext,
            IWebAuthenticationService authenticationService)
        {
            await authenticationService.Authenticate<TokenCredentials>(httpContext);
            await next(httpContext);
        }
    }
}