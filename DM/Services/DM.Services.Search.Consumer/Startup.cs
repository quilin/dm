using System;
using Autofac;
using DM.Services.Core;
using DM.Services.Core.Configuration;
using DM.Services.Core.Dto.Enums;
using DM.Services.Core.Extensions;
using DM.Services.Core.Implementation;
using DM.Services.Core.Logging;
using DM.Services.DataAccess;
using DM.Services.MessageQueuing;
using DM.Services.MessageQueuing.Building;
using DM.Services.MessageQueuing.GeneralBus;
using DM.Services.MessageQueuing.RabbitMq.Configuration;
using DM.Services.Search.Consumer.Implementation;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
                .Configure<ConnectionStrings>(
                    configuration.GetSection(nameof(ConnectionStrings)).Bind)
                .AddDmLogging("DM.Search.Consumer");

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
            builder.RegisterModuleOnce<SearchEngineModule>();
        }

        /// <summary>
        /// Ready to work
        /// </summary>
        /// <param name="applicationBuilder"></param>
        /// <param name="consumerBuilder"></param>
        public void Configure(IApplicationBuilder applicationBuilder,
            IConsumerBuilder<InvokedEvent> consumerBuilder)
        {
            Console.WriteLine("[🚴] Starting search engine consumer");

            var parameters = new RabbitConsumerParameters("dm.search-engine", ProcessingOrder.Unmanaged)
            {
                ExchangeName = InvokedEventsTransport.ExchangeName,
                RoutingKeys = new[]
                {
                    EventType.ActivatedUser,
                    EventType.NewForumComment,
                    EventType.ChangedForumComment,
                    EventType.DeletedForumComment,
                    EventType.NewForumTopic,
                    EventType.ChangedForumTopic,
                    EventType.DeletedForumTopic,
                }.ToRoutingKeys(),
                QueueName = "dm.search-engine"
            };
            consumerBuilder.BuildRabbit<CompositeIndexer>(parameters).Start();

            Console.WriteLine($"[👂] Consumer is listening to {parameters.QueueName} queue");

            applicationBuilder
                .UseRouting()
                .UseEndpoints(route => route.MapControllers());
        }
    }
}