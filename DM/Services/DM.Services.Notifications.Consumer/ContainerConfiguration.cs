using System.IO;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using DM.Services.Core.Configuration;
using DM.Services.DataAccess;
using DM.Services.MessageQueuing;
using DM.Services.MessageQueuing.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DM.Services.Notifications.Consumer
{
    /// <summary>
    /// Configures container for mail sender MQ consumer
    /// </summary>
    public static class ContainerConfiguration
    {
        /// <summary>
        /// Configure service provider
        /// </summary>
        /// <returns></returns>
        public static AutofacServiceProvider ConfigureProvider()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false)
                .Build();

            var services = new ServiceCollection()
                .AddOptions()
                .Configure<ConnectionStrings>(
                    configuration.GetSection(nameof(ConnectionStrings)).Bind)
                .Configure<MessageConsumeConfiguration>(
                    configuration.GetSection(nameof(MessageConsumeConfiguration)).Bind)
                .Configure<MessagePublishConfiguration>(
                    configuration.GetSection(nameof(MessagePublishConfiguration)).Bind)
                .AddDbContext<DmDbContext>(options =>
                    options.UseNpgsql(configuration.GetConnectionString(nameof(DmDbContext))));

            var builder = new ContainerBuilder();

            builder.RegisterModule<MessageQueuingModule>();
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