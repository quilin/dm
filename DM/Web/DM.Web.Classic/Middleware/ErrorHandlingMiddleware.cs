using System;
using System.Net;
using System.Threading.Tasks;
using DM.Services.Core.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;

namespace DM.Web.Classic.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;

        public ErrorHandlingMiddleware(
            RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext,
            ILogger<ErrorHandlingMiddleware> logger)
        {
            try
            {
                await next(httpContext);
            }
            catch (Exception e)
            {
                int statusCode;
                string message;
                switch (e)
                {
                    case HttpException httpException:
                        statusCode = (int) httpException.StatusCode;
                        message = httpException.Message;
                        break;
                    default:
                        statusCode = (int) HttpStatusCode.InternalServerError;
                        message = e.Message;
                        break;
                }

                logger.LogCritical(e, message);

                if (httpContext.Request.Path.HasValue &&
                    httpContext.Request.Path.Value.Contains("/error/"))
                {
                    return;
                }

                var path = httpContext.Request.GetEncodedUrl();
                httpContext.Response.StatusCode = statusCode;
                httpContext.Response.Redirect($"/error/{statusCode}/?path={path}");
            }
        }
    }
}