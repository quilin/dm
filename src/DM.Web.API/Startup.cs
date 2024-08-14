using System;
using Autofac;
using DM.Services.Common;
using DM.Services.Community;
using DM.Services.Core.Configuration;
using DM.Services.Core.Extensions;
using DM.Services.Core.Logging;
using DM.Services.Core.Parsing;
using DM.Services.DataAccess;
using DM.Services.Forum;
using DM.Services.Gaming;
using DM.Services.MessageQueuing;
using DM.Services.Notifications;
using DM.Services.Uploading;
using DM.Services.Uploading.Configuration;
using DM.Web.API.Authentication;
using DM.Web.API.Binding;
using DM.Web.API.Configuration;
using DM.Web.API.Middleware;
using DM.Web.API.Notifications;
using DM.Web.API.Swagger;
using DM.Web.Core;
using DM.Web.Core.Middleware;
using Jamq.Client.DependencyInjection;
using Jamq.Client.Rabbit;
using Jamq.Client.Rabbit.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DM.Web.API;

/// <summary>
/// Application
/// </summary>
internal class Startup(IConfiguration configuration)
{
    private IHttpContextAccessor httpContextAccessor;
    private IBbParserProvider bbParserProvider;
    private bool migrateOnStart;

    /// <summary>
    /// Configure application services
    /// </summary>
    /// <param name="services">Service collection</param>
    public void ConfigureServices(IServiceCollection services)
    {
        migrateOnStart = configuration.GetValue<bool>("MigrateOnStart");

        services
            .AddOptions()
            .Configure<ConnectionStrings>(configuration.GetSection(nameof(ConnectionStrings)).Bind)
            .Configure<IntegrationSettings>(configuration.GetSection(nameof(IntegrationSettings)).Bind)
            .Configure<EmailConfiguration>(configuration.GetSection(nameof(EmailConfiguration)).Bind)
            .Configure<CdnConfiguration>(configuration.GetSection(nameof(CdnConfiguration)).Bind)
            .Configure<RabbitMqConfiguration>(configuration.GetSection(nameof(RabbitMqConfiguration)).Bind)
            .Configure<SearchServiceConfiguration>(configuration.GetSection(nameof(SearchServiceConfiguration)).Bind)
            .AddDmLogging("DM.API", configuration);

        services
            .AddAutoMapper(config => config.AllowNullCollections = true)
            .AddMemoryCache()
            .AddDbContextPool<DmDbContext>(options => options
                .UseNpgsql(configuration.GetConnectionString(nameof(ConnectionStrings.Rdb))));

        services.AddJamqClient(config => config
            .UseRabbit(sp =>
            {
                var cfg = sp.GetRequiredService<IOptions<RabbitMqConfiguration>>().Value;
                return new RabbitConnectionParameters(cfg.Endpoint, cfg.VirtualHost, cfg.Username, cfg.Password);
            }));
        services.AddHostedService<RealtimeNotificationConsumer>();

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
        builder.RegisterModuleOnce<UploadingModule>();
        builder.RegisterModuleOnce<DataAccessModule>();

        builder.RegisterModuleOnce<CommunityModule>();
        builder.RegisterModuleOnce<ForumModule>();
        builder.RegisterModuleOnce<GamingModule>();
        builder.RegisterModuleOnce<NotificationsModule>();

        builder.RegisterModuleOnce<WebCoreModule>();
    }

    /// <summary>
    /// Configure application
    /// </summary>
    /// <param name="appBuilder"></param>
    /// <param name="integrationOptions"></param>
    /// <param name="dbContext"></param>
    /// <param name="logger"></param>
    public void Configure(IApplicationBuilder appBuilder,
        IOptions<IntegrationSettings> integrationOptions,
        DmDbContext dbContext,
        ILogger<Startup> logger)
    {
        if (migrateOnStart)
        {
            dbContext.Database.Migrate();
            Environment.Exit(0);
        }
        
        appBuilder
            .UseSwagger(c => c.Configure())
            .UseSwaggerUI(c => c.ConfigureUi())
            .UseMiddleware<CorrelationMiddleware>()
            .UseMiddleware<ErrorHandlingMiddleware>()
            .UseMiddleware<AuthenticationMiddleware>()
            .UseCors(b => b
                .WithOrigins(integrationOptions.Value.CorsUrls)
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
    }
}