using Autofac;
using DM.Services.Core;
using DM.Services.Core.Configuration;
using DM.Services.Core.Extensions;
using DM.Services.Core.Logging;
using DM.Services.MessageQueuing;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RMQ.Client.Abstractions;
using RMQ.Client.Abstractions.Consuming;
using RMQ.Client.DependencyInjection;
using ConfigurationFactory = DM.Services.Core.Configuration.ConfigurationFactory;

namespace DM.Services.Mail.Sender.Consumer
{
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
                .Configure<EmailConfiguration>(configuration.GetSection(nameof(EmailConfiguration)).Bind)
                .Configure<ConnectionStrings>(configuration.GetSection(nameof(ConnectionStrings)).Bind)
                .Configure<RabbitMqConfiguration>(configuration.GetSection(nameof(RabbitMqConfiguration)).Bind)
                .AddDmLogging("DM.MailSender.Consumer");

            services.AddRmqClient(sp =>
            {
                var cfg = sp.GetRequiredService<IOptions<RabbitMqConfiguration>>().Value;
                return new RabbitConnectionParameters(cfg.Endpoint, cfg.Username, cfg.Password);
            }, consumerBuilderDefaults: builder => builder.WithMiddleware<ConsumerRetryMiddleware>());
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
}