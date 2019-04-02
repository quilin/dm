using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;

namespace DM.Web.API.Middleware
{
    /// <summary>
    /// Middleware to log every request
    /// </summary>
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate next;

        /// <inheritdoc />
        public RequestLoggingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        /// <summary>
        /// Before request
        /// </summary>
        /// <param name="httpContext">HTTP context</param>
        /// <param name="logger">Logger</param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext httpContext,
            ILogger<RequestLoggingMiddleware> logger)
        {
            logger.LogWarning("API has been called: {Url}", httpContext.Request.GetEncodedUrl());
            await next(httpContext);
        }
    }
}