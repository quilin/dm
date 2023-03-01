using Autofac;
using DM.Services.Core;
using DM.Services.Core.Configuration;
using DM.Services.Core.Extensions;
using DM.Services.Core.Logging;
using DM.Services.MessageQueuing;
using Jamq.Client.Abstractions.Consuming;
using Jamq.Client.DependencyInjection;
using Jamq.Client.Rabbit;
using Jamq.Client.Rabbit.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace DM.Services.Mail.Sender.Consumer;

/// <summary>
/// Search consumer API configuration
/// </summary>
public class Startup
{
    private readonly IConfiguration configuration;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="configuration"></param>
    public Startup(IConfiguration configuration)
    {
        this.configuration = configuration;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public void ConfigureServices(IServiceCollection services)
    {
        services
            .AddOptions()
            .Configure<EmailConfiguration>(configuration.GetSection(nameof(EmailConfiguration)).Bind)
            .Configure<ConnectionStrings>(configuration.GetSection(nameof(ConnectionStrings)).Bind)
            .Configure<RabbitMqConfiguration>(configuration.GetSection(nameof(RabbitMqConfiguration)).Bind)
            .AddDmLogging("DM.MailSender.Consumer", configuration);

        services.AddJamqClient(config => config
            .UseRabbit(sp =>
            {
                var cfg = sp.GetRequiredService<IOptions<RabbitMqConfiguration>>().Value;
                return new RabbitConnectionParameters(cfg.Endpoint, cfg.Username, cfg.Password);
            }), consumerBuilderDefaults: builder => builder.WithMiddleware<ConsumerRetryMiddleware>());
        services.AddHostedService<MailSendingConsumer>();

        services.AddHealthChecks();

        services.AddMvc();
    }

    /// <summary>
    /// Configure application container
    /// </summary>
    /// <param name="builder">Container builder</param>
    public void ConfigureContainer(ContainerBuilder builder)
    {
        builder.RegisterDefaultTypes();

        builder.RegisterModuleOnce<CoreModule>();
        builder.RegisterModuleOnce<MailSenderModule>();
        builder.RegisterModuleOnce<MessageQueuingModule>();
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