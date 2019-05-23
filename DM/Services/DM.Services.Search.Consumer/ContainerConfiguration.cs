using System.IO;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using DM.Services.Core.Configuration;
using DM.Services.Core.Logging;
using DM.Services.DataAccess;
using DM.Services.MessageQueuing;
using DM.Services.MessageQueuing.Configuration;
using DM.Services.Search.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DM.Services.Search.Consumer
{
    /// <summary>
    /// Configures container for search engine MQ consumer
    /// </summary>
    public static class ContainerConfiguration
    {
        /// <summary>
        /// Create service provider for search engine MQ consumer
        /// </summary>
        /// <returns>Service provider</returns>
        public static AutofacServiceProvider ConfigureProvider()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false)
                .Build();

            var services = new ServiceCollection()
                .AddOptions()
                .AddDmLogging()
                .Configure<ConnectionStrings>(
                    configuration.GetSection(nameof(ConnectionStrings)).Bind)
                .Configure<MessageConsumeConfiguration>(
                    configuration.GetSection(nameof(MessageConsumeConfiguration)).Bind)
                .Configure<SearchEngineConfiguration>(
                    configuration.GetSection(nameof(SearchEngineConfiguration)).Bind)
                .AddDbContext<DmDbContext>(options =>
                    options.UseNpgsql(configuration.GetConnectionString(nameof(DmDbContext))));

            var builder = new ContainerBuilder();

            builder.RegisterModule<MessageQueuingModule>();
            builder.RegisterModule<SearchEngineModule>();
            builder.RegisterAssemblyTypes(
                    Assembly.GetExecutingAssembly(),
                    Assembly.Load("DM.Services.Core"))
                .Where(t => t.IsClass)
                .AsSelf()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.Populate(services);

            var container = builder.Build();
            return new AutofacServiceProvider(container);
        }
    }
}