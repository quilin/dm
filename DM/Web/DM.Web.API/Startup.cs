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
using DM.Services.MessageQueuing.Configuration;
using DM.Services.MessageQueuing.Consume;
using DM.Services.Notifications;
using DM.Services.Notifications.Dto;
using DM.Services.Search;
using DM.Web.API.Authentication;
using DM.Web.API.Binding;
using DM.Web.API.Configuration;
using DM.Web.API.Middleware;
using DM.Web.API.Notifications;
using DM.Web.API.Swagger;
using DM.Web.Core;
using DM.Web.Core.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
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
                .AddDbContextPool<DmDbContext>(options => options
                    .UseNpgsql(Configuration.GetConnectionString(nameof(ConnectionStrings.Rdb))));

            services.AddHealthChecks();

            httpContextAccessor = new HttpContextAccessor();
            bbParserProvider = new BbParserProvider();

            services.AddSignalR();

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
        /// <param name="notificationConsumer"></param>
        public void Configure(IApplicationBuilder appBuilder,
            IMessageConsumer<RealtimeNotification> notificationConsumer)
        {
            appBuilder
                .UseSwagger(c => c.Configure())
                .UseSwaggerUI(c => c.ConfigureUi())
                .UseMiddleware<CorrelationMiddleware>()
                .UseMiddleware<ErrorHandlingMiddleware>()
                .UseMiddleware<AuthenticationMiddleware>()
                .UseCors(b => b
                    .WithOrigins("http://localhost:8080")
                    .WithExposedHeaders(ApiCredentialsStorage.HttpAuthTokenHeader)
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials())
                .UseRouting()
                .UseHealthChecks("/_health")
                .UseEndpoints(c =>
                {
                    c.MapControllers();
                    c.MapHub<NotificationHub>("/whatsup");
                });

            notificationConsumer.Consume(new MessageConsumeConfiguration
            {
                ExchangeName = "dm.notifications.sent",
                QueueName = "dm.notifications.api",
                RoutingKeys = new[] {"#"},
                PrefetchCount = 1,
                Exclusive = true,
                ConsumerTag = "dm.api"
            });
        }
    }
}