using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;
using Serilog.Context;

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
            LogContext.PushProperty("URL", httpContext.Request.GetEncodedPathAndQuery());
            var stopwatch = Stopwatch.StartNew();
            await next(httpContext);
            stopwatch.Stop();
            logger.LogInformation("Request took {elapsed}ms", stopwatch.ElapsedMilliseconds);
        }
    }
}