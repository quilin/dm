using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using DM.Services.Core.Configuration;
using DM.Services.Core.Logging;
using DM.Services.MessageQueuing;
using DM.Services.MessageQueuing.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DM.Services.Mail.Sender.Consumer
{
    /// <summary>
    /// Configures container for mail sender MQ consumer
    /// </summary>
    public static class ContainerConfiguration
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
        /// Configure service provider for the consumer
        /// </summary>
        /// <returns></returns>
        public static AutofacServiceProvider ConfigureProvider()
        {
            var configuration = ConfigurationFactory.Default;

            var services = new ServiceCollection()
                .AddOptions()
                .Configure<MessageConsumeConfiguration>(
                    configuration.GetSection(nameof(MessageConsumeConfiguration)).Bind)
                .Configure<EmailConfiguration>(
                    configuration.GetSection(nameof(EmailConfiguration)).Bind)
                .Configure<ConnectionStrings>(
                    configuration.GetSection(nameof(ConnectionStrings)).Bind)
                .AddDmLogging("DM.MailSender");

            var builder = new ContainerBuilder();
            builder.RegisterModule<MessageQueuingModule>();
            builder.RegisterAssemblyTypes(GetAssemblies())
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