using System;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using DM.Services.Core.Configuration;
using DM.Services.Core.Dto.Enums;
using DM.Services.Core.Extensions;
using DM.Services.Core.Implementation;
using DM.Services.Core.Logging;
using DM.Services.DataAccess;
using DM.Services.DataAccess.MongoIntegration;
using DM.Services.MessageQueuing;
using DM.Services.MessageQueuing.Configuration;
using DM.Services.MessageQueuing.Consume;
using DM.Services.MessageQueuing.Dto;
using DM.Services.MessageQueuing.Publish;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            var configuration = ConfigurationFactory.Default;
            services
                .AddOptions()
                .Configure<ConnectionStrings>(
                    configuration.GetSection(nameof(ConnectionStrings)).Bind)
                .Configure<MessageConsumeConfiguration>(
                    configuration.GetSection(nameof(MessageConsumeConfiguration)).Bind)
                .AddDmLogging("DM.Search.Consumer");

            services
                .AddEntityFrameworkNpgsql()
                .AddDbContext<DmDbContext>(options => options
                    .UseNpgsql(configuration.GetConnectionString(nameof(ConnectionStrings.Rdb))));
            services
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            var assemblies = GetAssemblies();
            var builder = new ContainerBuilder();
            builder.RegisterAssemblyTypes(assemblies)
                .Where(t => t.IsClass)
                .AsSelf()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
            builder.RegisterTypes(typeof(DmMongoClient))
                .AsSelf()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterModuleOnce<MessageQueuingModule>();
            builder.RegisterModuleOnce<SearchEngineModule>();
            builder.Populate(services);

            var container = builder.Build();
            return new AutofacServiceProvider(container);
        }

        /// <summary>
        /// Ready to work
        /// </summary>
        /// <param name="applicationBuilder"></param>
        /// <param name="consumer"></param>
        public void Configure(IApplicationBuilder applicationBuilder,
            IMessageConsumer<InvokedEvent> consumer)
        {
            Console.WriteLine("[🚴] Starting search engine consumer");
            Console.WriteLine("[🔧] Configuring service provider");

            var configuration = new MessageConsumeConfiguration
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
            consumer.Consume(configuration);

            Console.WriteLine($"[👂] Consumer is listening to {configuration.QueueName} queue");

            applicationBuilder
                .UseHsts()
                .UseHttpsRedirection()
                .UseMvc();
        }
    }
}