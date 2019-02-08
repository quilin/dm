using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using DM.Services.Authentication.Implementation;
using DM.Services.Common.Implementation;
using DM.Services.Core.Configuration;
using DM.Services.Core.Implementation;
using DM.Services.DataAccess;
using DM.Services.DataAccess.MongoIntegration;
using DM.Services.Forum.Implementation;
using DM.Web.Core.Binders;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DM.Web.Core
{
    public abstract class BaseStartup
    {
        protected IConfigurationRoot Configuration { get; private set; }

        private static readonly Assembly[] ExternalAssemblyTypes = new[]
            {
                typeof(BaseStartup),
                typeof(DmDbContext),
                typeof(IGuidFactory),
                typeof(IIntentionManager),
                typeof(IAuthenticationService),
                typeof(IForumService)
            }
            .Select(t => t.Assembly)
            .Distinct()
            .ToArray();
        
        protected abstract IEnumerable<Type> ExternalSiteTypes { get; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            services
                .AddOptions()
                .AddMemoryCache()
                .AddEntityFrameworkNpgsql()
                .AddDbContext<DmDbContext>(options => options
                    .UseNpgsql(Configuration.GetConnectionString(nameof(DmDbContext))))
                .AddDbContext<ReadDmDbContext>(options => options
                    .UseNpgsql(Configuration.GetConnectionString(nameof(DmDbContext)))
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));
            AddConfiguration(services)
                .Configure<ConnectionStrings>(Configuration.GetSection(nameof(ConnectionStrings)).Bind)
                .Configure<IntegrationSettings>(Configuration.GetSection(nameof(IntegrationSettings)).Bind)
                .Configure<EmailConfiguration>(Configuration.GetSection(nameof(EmailConfiguration)).Bind)
                .AddMvc(config => config.ModelBinderProviders.Insert(0, new ReadableGuidBinderProvider()))
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterAssemblyTypes(ExternalAssemblyTypes
                    .Union(ExternalSiteTypes
                        .Select(t => t.Assembly)
                        .Distinct())
                    .ToArray())
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
            UseMiddleware(appBuilder);
            appBuilder
                .UseHsts()
                .UseHttpsRedirection()
                .UseStaticFiles()
                .UseCookiePolicy()
                .UseMvc(Route);
        }

        protected virtual IServiceCollection AddConfiguration(IServiceCollection services) => services;
        protected abstract IApplicationBuilder UseMiddleware(IApplicationBuilder appBuilder);

        protected virtual Action<IRouteBuilder> Route => _ => { };
    }
}