using System;
using System.Net;
using System.Threading.Tasks;
using DM.Services.Core.Exceptions;
using DM.Web.Classic.Extensions.RequestExtensions;
using Microsoft.AspNetCore.Http;
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

                if (httpContext.Request.IsAjaxRequest())
                {
                    httpContext.Response.StatusCode = statusCode;
                    return;
                }

                if (httpContext.Request.Path.HasValue &&
                    httpContext.Request.Path.Value.Contains("/error/"))
                {
                    return;
                }

                httpContext.Response.StatusCode = statusCode;
                httpContext.Response.Redirect($"/error/{statusCode}/?path={httpContext.Request.Path}");
            }
        }
    }
}