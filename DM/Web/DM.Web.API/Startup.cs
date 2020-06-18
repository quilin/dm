using Autofac;
using AutoMapper;
using DM.Services.Common;
using DM.Services.Community;
using DM.Services.Core.Configuration;
using DM.Services.Core.Extensions;
using DM.Services.Core.Logging;
using DM.Services.Core.Parsing;
using DM.Services.DataAccess;
using DM.Services.Forum;
using DM.Services.Gaming;
using DM.Services.Notifications;
using DM.Services.Search;
using DM.Web.API.Configuration;
using DM.Web.API.Middleware;
using DM.Web.API.Swagger;
using DM.Web.Core;
using DM.Web.Core.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ReadableGuidBinderProvider = DM.Web.API.Binding.ReadableGuidBinderProvider;

namespace DM.Web.API
{
    /// <summary>
    /// Application
    /// </summary>
    public class Startup
    {
        /// <inheritdoc />
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; set; }
        private IHttpContextAccessor httpContextAccessor;
        private IBbParserProvider bbParserProvider;

        /// <summary>
        /// Configure application services
        /// </summary>
        /// <param name="services">Service collection</param>
        public void ConfigureServices(IServiceCollection services)
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

            services
                .AddAutoMapper(config => config.AllowNullCollections = true)
                .AddMemoryCache()
                .AddDbContext<DmDbContext>(options => options
                        .UseNpgsql(Configuration.GetConnectionString(nameof(ConnectionStrings.Rdb))),
                    ServiceLifetime.Transient);

            services.AddHealthChecks();

            httpContextAccessor = new HttpContextAccessor();
            bbParserProvider = new BbParserProvider();

            services
                .AddSwaggerGen(c => c.ConfigureGen())
                .AddMvc(config => config.ModelBinderProviders.Insert(0, new ReadableGuidBinderProvider()))
                .AddJsonOptions(config => config.Setup(httpContextAccessor, bbParserProvider));
        }

        /// <summary>
        /// Configure application container
        /// </summary>
        /// <param name="builder">Container builder</param>
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterDefaultTypes();
            builder.RegisterMapper();

            builder.RegisterInstance(httpContextAccessor)
                .AsSelf()
                .AsImplementedInterfaces();
            builder.RegisterInstance(bbParserProvider)
                .AsSelf()
                .AsImplementedInterfaces();

            builder.RegisterModuleOnce<CommonModule>();
            builder.RegisterModuleOnce<DataAccessModule>();

            builder.RegisterModuleOnce<CommunityModule>();
            builder.RegisterModuleOnce<ForumModule>();
            builder.RegisterModuleOnce<GamingModule>();
            builder.RegisterModuleOnce<NotificationsModule>();
            builder.RegisterModuleOnce<SearchEngineModule>();

            builder.RegisterModuleOnce<WebCoreModule>();
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
                .UseRouting()
                .UseHealthChecks("/_health")
                .UseEndpoints(c => c.MapControllers());
        }
    }
}