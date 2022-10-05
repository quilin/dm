using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using DM.Web.API.Authentication;
using DM.Web.API.BbRendering;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace DM.Web.API.Swagger;

/// <summary>
/// Extensions for swagger setup
/// </summary>
public static class SwaggerExtensions
{
    private static readonly IEnumerable<string> ApiGroups = Assembly.GetExecutingAssembly().GetTypes()
        .Where(t => t.IsSubclassOf(typeof(ControllerBase)))
        .Select(t => t.GetCustomAttribute<ApiExplorerSettingsAttribute>())
        .Where(t => t is {IgnoreApi: false})
        .Select(t => t.GroupName)
        .Distinct();
        
    /// <summary>
    /// Configure swagger gen
    /// </summary>
    /// <param name="options"></param>
    public static void ConfigureGen(this SwaggerGenOptions options)
    {
        foreach (var apiGroup in ApiGroups)
        {
            options.SwaggerDoc(apiGroup, new OpenApiInfo {Title = $"DM.API {apiGroup}", Version = "v1"});
        }

        options.OperationFilter<AuthenticationSwaggerFilter>();
        options.OperationFilter<BbRenderModeSwaggerFilter>();

        var apiAssemblyName = Assembly.GetExecutingAssembly().GetName().Name;
        options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{apiAssemblyName}.xml"));

        options.DescribeAllParametersInCamelCase();
    }

    /// <summary>
    /// Configure swagger usage
    /// </summary>
    /// <param name="options"></param>
    public static void Configure(this SwaggerOptions options)
    {
        options.RouteTemplate = "swagger/{documentName}/swagger.json";
        options.PreSerializeFilters.Add(ReverseProxyPreSerializeFilter);
    }

    /// <summary>
    /// Configure swagger UI
    /// </summary>
    /// <param name="options"></param>
    public static void ConfigureUi(this SwaggerUIOptions options)
    {
        foreach (var apiGroup in ApiGroups)
        {
            options.SwaggerEndpoint($"swagger/{apiGroup}/swagger.json", apiGroup);
        }

        options.RoutePrefix = string.Empty;
        options.DocumentTitle = "DM.API";
    }

    private const string ForwardedPrefixHeader = "X-Forwarded-Prefix";

    private static readonly Action<OpenApiDocument, HttpRequest> ReverseProxyPreSerializeFilter =
        (document, request) =>
        {
            string prefix;
            if (!request.Headers.TryGetValue(ForwardedPrefixHeader, out var prefixHeaderValues) ||
                !prefixHeaderValues.Any() ||
                string.IsNullOrEmpty(prefix = prefixHeaderValues.First()))
            {
                return;
            }

            document.Servers.Add(new OpenApiServer
            {
                Url = prefix,
                Description = "Reverse proxy server",
            });
        };
}