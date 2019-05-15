using System.IO;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using DM.Services.Core.Configuration;
using DM.Services.MessageQueuing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DM.Services.Mail.Sender.Consumer
{
    /// <summary>
    /// Configures container for mail sender MQ consumer
    /// </summary>
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
                .Configure<EmailConfiguration>(
                    configuration.GetSection(nameof(EmailConfiguration)).Bind);
            
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