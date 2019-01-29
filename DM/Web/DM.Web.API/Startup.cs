using System;
using System.Collections.Generic;
using DM.Web.Core;
using DM.Web.Core.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace DM.Web.API
{
    public class Startup : BaseStartup
    {
        protected override IEnumerable<Type> ExternalSiteTypes => new[]
        {
            typeof(Startup)
        };

        protected override IApplicationBuilder UseMiddleware(IApplicationBuilder appBuilder) =>
            appBuilder
                .UseMiddleware<AuthenticationMiddleware>()
                .UseSwagger()
                .UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "DM.API V1");
                    c.RoutePrefix = string.Empty;
                    c.DocumentTitle = "DM.API";
                });

        protected override IServiceCollection AddConfiguration(IServiceCollection services)
        {
            return services
                .AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new Info {Title = "DM.API", Version = "v1"});
                    c.DescribeAllParametersInCamelCase();
                    c.DescribeAllEnumsAsStrings();
                });
        }
    }
}