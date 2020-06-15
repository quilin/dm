using DM.Services.Core.Parsing;
using DM.Web.API.BbRendering;
using DM.Web.API.Middleware;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace DM.Web.API.Configuration
{
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
        public static void Setup(this MvcJsonOptions config,
            IHttpContextAccessor httpContextAccessor,
            IBbParserProvider bbParserProvider)
        {
            config.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            config.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            config.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            config.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
            config.SerializerSettings.DateFormatString = "O";

            config.SerializerSettings.Converters.Insert(0, new StringEnumConverter());
            config.SerializerSettings.Converters.Insert(0, new ReadableGuidConverter());
            config.SerializerSettings.Converters.Insert(0, new ReadableNullableGuidConverter());
            config.SerializerSettings.Converters.Insert(0, new OptionalConverter());
            config.SerializerSettings.Converters.Insert(0, new BbConverter(httpContextAccessor, bbParserProvider));
        }
    }
}