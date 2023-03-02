using Autofac;
using DM.Services.Core;
using DM.Services.Core.Configuration;
using DM.Services.Core.Extensions;
using DM.Services.Core.Logging;
using DM.Services.DataAccess;
using DM.Services.MessageQueuing;
using Jamq.Client.DependencyInjection;
using Jamq.Client.Rabbit;
using Jamq.Client.Rabbit.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ConfigurationFactory = DM.Services.Core.Configuration.ConfigurationFactory;

namespace DM.Services.Notifications.Consumer;

/// <summary>
/// Search consumer API configuration
/// </summary>
public class Startup
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public void ConfigureServices(IServiceCollection services)
    {
        var configuration = ConfigurationFactory.Default;
        services
            .AddOptions()
            .Configure<ConnectionStrings>(configuration.GetSection(nameof(ConnectionStrings)).Bind)
            .Configure<RabbitMqConfiguration>(configuration.GetSection(nameof(RabbitMqConfiguration)).Bind)
            .AddDmLogging("DM.Notifications.Consumer");

        services.AddJamqClient(config => config
            .UseRabbit(sp =>
            {
                var cfg = sp.GetRequiredService<IOptions<RabbitMqConfiguration>>().Value;
                return new RabbitConnectionParameters(cfg.Endpoint, cfg.VirtualHost, cfg.Username, cfg.Password);
            }));
        services.AddHostedService<NotificationConsumer>();

        services.AddHealthChecks();

        services
            .AddDbContext<DmDbContext>(options => options
                .UseNpgsql(configuration.GetConnectionString(nameof(ConnectionStrings.Rdb))))
            .AddMvc();
    }

    /// <summary>
    /// Configure application container
    /// </summary>
    /// <param name="builder">Container builder</param>
    public void ConfigureContainer(ContainerBuilder builder)
    {
        builder.RegisterDefaultTypes();

        builder.RegisterModuleOnce<CoreModule>();
        builder.RegisterModuleOnce<DataAccessModule>();
        builder.RegisterModuleOnce<MessageQueuingModule>();
        builder.RegisterModuleOnce<NotificationsModule>();
    }

    /// <summary>
    /// Ready to work
    /// </summary>
    /// <param name="applicationBuilder"></param>
    public void Configure(IApplicationBuilder applicationBuilder)
    {
        applicationBuilder
            .UseRouting()
            .UseHealthChecks("/_health")
            .UseEndpoints(route => route.MapControllers());
    }
}