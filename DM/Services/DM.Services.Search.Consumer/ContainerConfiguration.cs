using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using DM.Services.Core.Configuration;
using DM.Services.Core.Logging;
using DM.Services.DataAccess;
using DM.Services.MessageQueuing;
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
        /// Create service provider for search engine MQ consumer
        /// </summary>
        /// <returns>Service provider</returns>
        public static AutofacServiceProvider ConfigureProvider()
        {
            var configuration = ConfigurationFactory.Default;

            var services = new ServiceCollection()
                .AddOptions()
                .Configure<ConnectionStrings>(
                    configuration.GetSection(nameof(ConnectionStrings)).Bind)
                .AddDbContext<DmDbContext>(options =>
                    options.UseNpgsql(configuration.GetConnectionString(nameof(ConnectionStrings.Rdb))))
                .AddDmLogging("DM.SearchEngine");

            var builder = new ContainerBuilder();

            builder.RegisterModule<MessageQueuingModule>();
            builder.RegisterModule<SearchEngineModule>();
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