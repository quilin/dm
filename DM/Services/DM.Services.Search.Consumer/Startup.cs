using Autofac;
using DM.Services.Core;
using DM.Services.Core.Configuration;
using DM.Services.Core.Extensions;
using DM.Services.Core.Logging;
using DM.Services.DataAccess;
using DM.Services.MessageQueuing;
using DM.Services.Search.Consumer.Implementation;
using DM.Services.Search.Consumer.Interceptors;
using Jamq.Client.DependencyInjection;
using Jamq.Client.Rabbit;
using Jamq.Client.Rabbit.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ConfigurationFactory = DM.Services.Core.Configuration.ConfigurationFactory;

namespace DM.Services.Search.Consumer
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
                .Configure<ConnectionStrings>(configuration.GetSection(nameof(ConnectionStrings)).Bind)
                .Configure<RabbitMqConfiguration>(configuration.GetSection(nameof(RabbitMqConfiguration)).Bind)
                .AddDmLogging("DM.Search.Consumer");

            services.AddJamqClient(config => config
                .UseRabbit(sp =>
                {
                    var cfg = sp.GetRequiredService<IOptions<RabbitMqConfiguration>>().Value;
                    return new RabbitConnectionParameters(cfg.Endpoint, cfg.Username, cfg.Password);
                }));
            services.AddHostedService<SearchEngineConsumer>();

            services.AddDbContext<DmDbContext>(options => options
                .UseNpgsql(configuration.GetConnectionString(nameof(ConnectionStrings.Rdb))));

            services.AddMvc();
            services.AddGrpc(options => options.Interceptors.Add<IdentityInterceptor>());
        }

        /// <summary>
        /// Configure application container
        /// </summary>
        /// <param name="builder">Container builder</param>
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterDefaultTypes();
            builder.RegisterMapper();

            builder.RegisterModuleOnce<CoreModule>();
            builder.RegisterModuleOnce<DataAccessModule>();
            builder.RegisterModuleOnce<MessageQueuingModule>();
            builder.RegisterModuleOnce<SearchEngineModule>();
        }

        /// <summary>
        /// Ready to work
        /// </summary>
        /// <param name="applicationBuilder"></param>
        /// <param name="logger"></param>
        public void Configure(IApplicationBuilder applicationBuilder,
            ILogger<Startup> logger)
        {
            applicationBuilder
                .UseRouting()
                .UseEndpoints(route => route.MapGrpcService<SearchEngineService>());
        }
    }
}