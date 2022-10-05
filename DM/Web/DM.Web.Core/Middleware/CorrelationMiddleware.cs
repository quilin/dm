using System;
using System.Linq;
using System.Threading.Tasks;
using DM.Services.Core.Implementation;
using DM.Services.Core.Implementation.CorrelationToken;
using Microsoft.AspNetCore.Http;

namespace DM.Web.Core.Middleware;

/// <summary>
/// Middleware for correlation token provider
/// </summary>
public class CorrelationMiddleware
{
    private readonly RequestDelegate next;
    private const string CorrelationTokenHeader = "X-Dm-Correlation-Token";

    /// <inheritdoc />
    public CorrelationMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    /// <summary>
    /// Before request
    /// </summary>
    /// <param name="httpContext"></param>
    /// <param name="setter"></param>
    /// <param name="guidFactory"></param>
    /// <returns></returns>
    public async Task InvokeAsync(HttpContext httpContext,
        ICorrelationTokenSetter setter,
        IGuidFactory guidFactory)
    {
        var correlationToken = httpContext.Request.Headers.TryGetValue(CorrelationTokenHeader, out var tokens) &&
                               Guid.TryParse(tokens.FirstOrDefault(), out var token)
            ? token
            : guidFactory.Create();
        setter.Current = correlationToken;
        await next(httpContext);
    }
}