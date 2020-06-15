using System;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using DM.Services.Core.Configuration;
using DM.Services.Core.Logging;
using DM.Services.Core.Parsing;
using DM.Services.DataAccess;
using DM.Web.API.Configuration;
using DM.Web.API.Middleware;
using DM.Web.API.Swagger;
using DM.Web.Core.Binders;
using DM.Web.Core.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DM.Web.API
{
    /// <summary>
    /// Application
    /// </summary>
    public class Startup
    {
        private IConfigurationRoot Configuration { get; set; }

        private static Assembly[] GetAssemblies()
        {
            var currentAssembly = Assembly.GetExecutingAssembly();
            var referencedAssemblies = currentAssembly.GetReferencedAssemblies().Select(Assembly.Load).ToArray();
            return referencedAssemblies
                .Union(new[] {currentAssembly})
                .Union(referencedAssemblies.SelectMany(a => a.GetReferencedAssemblies().Select(Assembly.Load)))
                .Where(a => a.FullName.StartsWith("DM."))
                .Distinct()
                .ToArray();
        }

        /// <summary>
        /// Configure application services
        /// </summary>
        /// <param name="services">Service collection</param>
        /// <returns>Service provider</returns>
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            Configuration = ConfigurationFactory.Default;

            services
                .AddOptions()
                .Configure<ConnectionStrings>(
                    Configuration.GetSection(nameof(ConnectionStrings)).Bind)
                .Configure<IntegrationSettings>(
                    Configuration.GetSection(nameof(IntegrationSettings)).Bind)
                .Configure<EmailConfiguration>(
                    Configuration.GetSection(nameof(EmailConfiguration)).Bind)
                .AddDmLogging("DM.API");

            var assemblies = GetAssemblies();
            services
                .AddAutoMapper(config => config.AllowNullCollections = true, assemblies)
                .AddMemoryCache()
                .AddEntityFrameworkNpgsql()
                .AddDbContext<DmDbContext>(options => options
                    .UseNpgsql(Configuration.GetConnectionString(nameof(ConnectionStrings.Rdb))), ServiceLifetime.Transient);

            var httpContextAccessor = new HttpContextAccessor();
            var bbParserProvider = new BbParserProvider();

            services
                .AddSwaggerGen(c => c.ConfigureGen())
                .AddMvc(config => config.ModelBinderProviders.Insert(0, new ReadableGuidBinderProvider()))
                .AddJsonOptions(config => config.Setup(httpContextAccessor, bbParserProvider))
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            var builder = new ContainerBuilder();

            builder.RegisterModule(new ApiModule(assemblies, httpContextAccessor, bbParserProvider));
            builder.Populate(services);

            var container = builder.Build();
            return new AutofacServiceProvider(container);
        }

        /// <summary>
        /// Configure application
        /// </summary>
        /// <param name="appBuilder"></param>
        public void Configure(IApplicationBuilder appBuilder)
        {
            appBuilder
                .UseSwagger(c => c.Configure())
                .UseSwaggerUI(c => c.ConfigureUi())
                .UseMiddleware<CorrelationMiddleware>()
                .UseMiddleware<ErrorHandlingMiddleware>()
                .UseMiddleware<AuthenticationMiddleware>()
                .UseCors(b => b
                    .WithExposedHeaders("X-Dm-Auth-Token")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowAnyOrigin())
                .UseMvc();
        }
    }
}