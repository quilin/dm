using System;
using System.IO;
using System.Reflection;
using DM.Web.API.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace DM.Web.API.Swagger
{
    /// <summary>
    /// Extensions for swagger setup
    /// </summary>
    public static class SwaggerExtensions
    {
        /// <summary>
        /// Configure swagger UI
        /// </summary>
        /// <param name="options"></param>
        public static void ConfigureUi(this SwaggerUIOptions options)
        {
            options.EnableValidator(null); // disable validator for encoded Guids
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "DM.API V1");
            options.RoutePrefix = string.Empty;
            options.DocumentTitle = "DM.API";
        }

        /// <summary>
        /// Configure swagger gen
        /// </summary>
        /// <param name="options"></param>
        public static void ConfigureGen(this SwaggerGenOptions options)
        {
            options.SwaggerDoc("v1", new Info {Title = "DM.API", Version = "v1"});
            options.DescribeAllParametersInCamelCase();
            options.DescribeAllEnumsAsStrings();
            options.OperationFilter<AuthenticationSwaggerFilter>();
            var apiAssemblyName = Assembly.GetExecutingAssembly().GetName().Name;
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{apiAssemblyName}.xml"));
        }
    }
}