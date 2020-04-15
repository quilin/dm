using System;
using System.Linq;
using System.Reflection;
using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using DM.Services.Common.Configuration;
using DM.Services.Core.Configuration;
using DM.Services.Core.Extensions;
using DM.Services.Core.Logging;
using DM.Services.DataAccess;
using DM.Services.DataAccess.MongoIntegration;
using DM.Services.MessageQueuing;
using DM.Services.Search;
using DM.Web.Classic.Configuration;
using DM.Web.Classic.Middleware;
using DM.Web.Core.Binders;
using DM.Web.Core.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace DM.Web.Classic
{
    public class Startup
    {
        private IConfiguration Configuration { get; set; }

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

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            Configuration = ConfigurationFactory.Default;

            services
                .AddOptions()
                .Configure<ConnectionStrings>(
                    Configuration.GetSection(nameof(ConnectionStrings)).Bind)
                .Configure<IntegrationSettings>(
                    Configuration.GetSection(nameof(IntegrationSettings)).Bind)
                .Configure<CdnConfiguration>(
                    Configuration.GetSection(nameof(CdnConfiguration)).Bind)
                .AddDmLogging("DM.Classic");
            var assemblies = GetAssemblies();

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services
                .AddAutoMapper(assemblies)
                .AddMemoryCache()
                .AddEntityFrameworkNpgsql()
                .AddDbContext<DmDbContext>(options =>
                        options.UseNpgsql(Configuration.GetConnectionString(nameof(ConnectionStrings.Rdb))),
                    ServiceLifetime.Transient);

            services
                .AddMvc(config => config.ModelBinderProviders.Insert(0, new ReadableGuidBinderProvider()))
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            var builder = new ContainerBuilder();
            builder.RegisterAssemblyTypes(assemblies)
                .Where(t => t.IsClass)
                .AsSelf()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
            builder.RegisterType<DmMongoClient>()
                .AsSelf()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.Register<IAmazonS3>(ctx =>
                {
                    var cdnConfiguration = ctx.Resolve<IOptions<CdnConfiguration>>().Value;
                    var s3Client = new AmazonS3Client(
                        new BasicAWSCredentials(cdnConfiguration.AccessKey, cdnConfiguration.SecretKey),
                        new AmazonS3Config
                        {
                            ServiceURL = cdnConfiguration.Url,
                            RegionEndpoint = RegionEndpoint.GetBySystemName(cdnConfiguration.Region)
                        });
                    return s3Client;
                })
                .AsSelf()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterModuleOnce<MessageQueuingModule>();
            builder.RegisterModuleOnce<SearchEngineModule>();

            builder.Populate(services);

            var container = builder.Build();
            return new AutofacServiceProvider(container);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder appBuilder, IHostingEnvironment env)
        {
            appBuilder
                .UseMiddleware<CorrelationMiddleware>()
                .UseMiddleware<ErrorHandlingMiddleware>()
                .UseMiddleware<AuthenticationMiddleware>();
            appBuilder
                .UseHsts()
                .UseHttpsRedirection()
                .UseStaticFiles()
                .UseCookiePolicy()
                .UseStatusCodePagesWithRedirects("/error/{0}")
                .UseMvc(RouteConfig.Register);
        }
    }
}