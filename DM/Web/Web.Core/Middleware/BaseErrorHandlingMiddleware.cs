using System;
using System.Net;
using System.Threading.Tasks;
using DM.Services.Core.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;

namespace Web.Core.Middleware
{
    public abstract class BaseErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;

        protected BaseErrorHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }
        
        protected virtual Task Redirect(HttpContext httpContext) => Task.CompletedTask;

        public async Task InvokeAsync(HttpContext httpContext,
            ILogger logger)
        {
            try
            {
                await next(httpContext);
            }
            catch (Exception e)
            {
                logger.Log(LogLevel.Error, e, httpContext.Request.GetDisplayUrl());
                switch (e)
                {
                    case HttpException httpException:
                        httpContext.Response.StatusCode = (int) httpException.StatusCode;
                        break;
                    default:
                        httpContext.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                        break;
                }

                await Redirect(httpContext);
            }
        }
    }
}