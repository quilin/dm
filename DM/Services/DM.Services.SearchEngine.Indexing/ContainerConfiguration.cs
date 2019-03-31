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
using RabbitMQ.Client;

namespace DM.Services.SearchEngine.Indexing
{
    public static class ContainerConfiguration
    {
        public static AutofacServiceProvider ConfigureProvider()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false)
                .Build();

            var services = new ServiceCollection()
                .AddOptions()
                .Configure<ConnectionStrings>(configuration.GetSection(nameof(ConnectionStrings)).Bind)
                .Configure<MessageConsumeConfiguration>(configuration.GetSection("ConsumerConfiguration").Bind)
                .AddDbContext<DmDbContext>(options =>
                    options.UseNpgsql(configuration.GetConnectionString(nameof(DmDbContext))));

            var builder = new ContainerBuilder();
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(t => t.IsClass)
                .AsSelf()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
            builder.RegisterType<IndexingEventsProcessor>()
                .AsImplementedInterfaces()
                .InstancePerDependency();
            builder.Register<IConnectionFactory>(x => new ConnectionFactory())
                .AsImplementedInterfaces()
                .InstancePerDependency();
            builder.RegisterGeneric(typeof(MessageConsumer<>))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
            builder.Populate(services);

            var container = builder.Build();
            return new AutofacServiceProvider(container);
        }
    }
}