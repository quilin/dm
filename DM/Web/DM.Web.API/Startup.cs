using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using DM.Services.Core.Configuration;
using DM.Services.DataAccess;
using DM.Services.DataAccess.MongoIntegration;
using DM.Web.API.Authentication;
using DM.Web.API.Middleware;
using DM.Web.Core.Binders;
using DM.Web.Core.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace DM.Web.API
{
    public class Startup
    {
        private IConfigurationRoot Configuration { get; set; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            services
                .AddOptions()
                .Configure<ConnectionStrings>(Configuration.GetSection(nameof(ConnectionStrings)).Bind)
                .Configure<IntegrationSettings>(Configuration.GetSection(nameof(IntegrationSettings)).Bind)
                .Configure<EmailConfiguration>(Configuration.GetSection(nameof(EmailConfiguration)).Bind)

                .AddMemoryCache()
                .AddEntityFrameworkNpgsql()
                .AddDbContext<DmDbContext>(options => options
                    .UseNpgsql(Configuration.GetConnectionString(nameof(DmDbContext))))
                .AddDbContext<ReadDmDbContext>(options => options
                    .UseNpgsql(Configuration.GetConnectionString(nameof(DmDbContext)))
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking))

                .AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new Info {Title = "DM.API", Version = "v1"});
                    c.DescribeAllParametersInCamelCase();
                    c.DescribeAllEnumsAsStrings();
                    c.OperationFilter<AuthenticationSwaggerFilter>();
                })

                .AddMvc(config => config.ModelBinderProviders.Insert(0, new ReadableGuidBinderProvider()))
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            
            var containerBuilder = new ContainerBuilder();
            var currentAssembly = Assembly.GetExecutingAssembly();
            var assemblies = currentAssembly.GetReferencedAssemblies()
                .Where(n => n.Name.StartsWith("DM."))
                .Select(Assembly.Load)
                .Union(new[] {currentAssembly})
                .ToArray();
            containerBuilder
                .RegisterAssemblyTypes(assemblies)
                .Where(t => t.IsClass)
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            containerBuilder.RegisterTypes(typeof(DmMongoClient))
                .AsSelf()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            containerBuilder.Populate(services);

            var container = containerBuilder.Build();
            return new AutofacServiceProvider(container);
        }
        
        public void Configure(IApplicationBuilder appBuilder)
        {
            appBuilder
                .UseMiddleware<ErrorHandlingMiddleware>()
                .UseMiddleware<AuthenticationMiddleware>()
                .UseSwagger()
                .UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "DM.API V1");
                    c.RoutePrefix = string.Empty;
                    c.DocumentTitle = "DM.API";
                })
                .UseMvc();
        }
    }
}