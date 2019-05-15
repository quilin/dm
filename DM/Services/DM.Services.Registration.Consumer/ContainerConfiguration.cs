using System.IO;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using DM.Services.Core.Configuration;
using DM.Services.DataAccess;
using DM.Services.Mail.Sender;
using DM.Services.MessageQueuing;
using DM.Services.MessageQueuing.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DM.Services.Registration.Consumer
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
                .Configure<ConnectionStrings>(
                    configuration.GetSection(nameof(ConnectionStrings)).Bind)
                .Configure<EmailConfiguration>(
                    configuration.GetSection(nameof(EmailConfiguration)).Bind)
                .Configure<MessageConsumeConfiguration>(
                    configuration.GetSection(nameof(MessageConsumeConfiguration)).Bind)
                .AddDbContext<DmDbContext>(options =>
                    options.UseNpgsql(configuration.GetConnectionString(nameof(DmDbContext))))
                .AddMailSender(configuration);

            var builder = new ContainerBuilder();

            builder.RegisterModule<MessageQueuingModule>();
            builder.RegisterModule<MailSenderModule>();
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