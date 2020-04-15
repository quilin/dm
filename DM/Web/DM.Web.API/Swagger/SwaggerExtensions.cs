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
            options.SwaggerEndpoint("/swagger/Account/swagger.json", "Account");
            options.SwaggerEndpoint("/swagger/Common/swagger.json", "Common");
            options.SwaggerEndpoint("/swagger/Forum/swagger.json", "Forum");
            options.SwaggerEndpoint("/swagger/Game/swagger.json", "Game");
            options.SwaggerEndpoint("/swagger/Community/swagger.json", "Community");

            options.RoutePrefix = string.Empty;
            options.DocumentTitle = "DM.API";
        }

        /// <summary>
        /// Configure swagger gen
        /// </summary>
        /// <param name="options"></param>
        public static void ConfigureGen(this SwaggerGenOptions options)
        {
            options.SwaggerDoc("Account", new Info {Title = "DM.API Account", Version = "v1"});
            options.SwaggerDoc("Common", new Info {Title = "DM.API Common", Version = "v1"});
            options.SwaggerDoc("Forum", new Info {Title = "DM.API Forum", Version = "v1"});
            options.SwaggerDoc("Game", new Info {Title = "DM.API Game", Version = "v1"});
            options.SwaggerDoc("Community", new Info {Title = "DM.API Community", Version = "v1"});
            options.DescribeAllParametersInCamelCase();
            options.DescribeAllEnumsAsStrings();
            options.OperationFilter<AuthenticationSwaggerFilter>();

            var apiAssemblyName = Assembly.GetExecutingAssembly().GetName().Name;
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{apiAssemblyName}.xml"));
        }
    }
}