using System;
using System.Linq;
using System.Threading.Tasks;
using DM.Services.Authentication.Implementation.CorrelationToken;
using DM.Services.Core.Implementation;
using Microsoft.AspNetCore.Http;

namespace DM.Web.Core.Middleware
{
    public class CorrelationMiddleware
    {
        private readonly RequestDelegate next;
        private const string CorrelationTokenHeader = "X-Dm-Correlation-Token";

        public CorrelationMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public Task InvokeAsync(HttpContext httpContext,
            ICorrelationTokenSetter setter,
            IGuidFactory guidFactory)
        {
            setter.Current =
                httpContext.Request.Headers.TryGetValue(CorrelationTokenHeader, out var tokens) &&
                Guid.TryParse(tokens.FirstOrDefault(), out var token)
                    ? token
                    : guidFactory.Create();
            return next(httpContext);
        }
    }
}