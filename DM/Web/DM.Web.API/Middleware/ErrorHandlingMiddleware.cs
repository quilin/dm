using System;
using System.Threading.Tasks;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Common.Authorization;
using DM.Services.Core.Exceptions;
using DM.Services.Core.Implementation.CorrelationToken;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Logging;

namespace DM.Web.API.Middleware;

/// <summary>
/// Middleware for exceptions handling
/// </summary>
internal class ErrorHandlingMiddleware
{
    private readonly RequestDelegate next;

    /// <inheritdoc />
    public ErrorHandlingMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    /// <summary>
    /// Before request
    /// </summary>
    /// <param name="httpContext">HTTP context</param>
    /// <param name="logger">Logger</param>
    /// <param name="identitySetter">Identity setter for Serilog issue fix</param>
    /// <param name="correlationTokenProvider">Correlation token for support assistance</param>
    /// <param name="problemDetailsFactory">Problem details factory</param>
    /// <returns></returns>
    public async Task InvokeAsync(HttpContext httpContext,
        ILogger<ErrorHandlingMiddleware> logger,
        IIdentitySetter identitySetter,
        ICorrelationTokenProvider correlationTokenProvider,
        ProblemDetailsFactory problemDetailsFactory)
    {
        try
        {
            await next(httpContext);
        }
        catch (Exception e)
        {
            identitySetter.Refresh();
            object error;
            switch (e)
            {
                case HttpBadRequestException badRequestException:
                    error = problemDetailsFactory.CreateFrom(badRequestException, httpContext);
                    break;
                case IntentionManagerException securityException:
                    logger.LogWarning(securityException, "Security breach attempt: {Message}", e.Message);
                    error = problemDetailsFactory.CreateFrom(securityException, httpContext);
                    break;
                case HttpException httpException:
                    error = problemDetailsFactory.CreateFrom(httpException, httpContext);
                    break;
                case NotImplementedException notImplementedException:
                    error = problemDetailsFactory.CreateFrom(notImplementedException, httpContext);
                    break;
                case ValidationException validationException:
                    error = problemDetailsFactory.CreateFrom(validationException, httpContext);
                    break;
                default:
                    logger.LogCritical(e, "Unhandled server error: {Message}", e.Message);
                    error = problemDetailsFactory.CreateFrom(e, httpContext, correlationTokenProvider.Current);
                    break;
            }

            httpContext.Response.StatusCode = error is ProblemDetails { Status: not null } problemDetails
                ? problemDetails.Status.Value
                : StatusCodes.Status500InternalServerError;
            await httpContext.Response.WriteAsJsonAsync(error);
        }
    }
}