using System.Text.Json;
using System.Text.Json.Serialization;
using DM.Services.Core.Parsing;
using DM.Web.API.BbRendering;
using DM.Web.API.Binding;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.API.Configuration;

/// <summary>
/// Configuration of API JSON serializer
/// </summary>
public static class JsonConfiguration
{
    /// <summary>
    /// Setup JSON API options
    /// </summary>
    /// <param name="config"></param>
    /// <param name="httpContextAccessor"></param>
    /// <param name="bbParserProvider"></param>
    public static void Setup(this JsonOptions config,
        IHttpContextAccessor httpContextAccessor,
        IBbParserProvider bbParserProvider)
    {
        config.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        config.JsonSerializerOptions.PropertyNameCaseInsensitive = true;

        config.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;

        config.JsonSerializerOptions.Converters.Insert(0, new JsonStringEnumConverter());
        config.JsonSerializerOptions.Converters.Insert(0, new OptionalConverterFactory());
        config.JsonSerializerOptions.Converters.Insert(0, new BbConverterFactory(httpContextAccessor, bbParserProvider));
    }
}