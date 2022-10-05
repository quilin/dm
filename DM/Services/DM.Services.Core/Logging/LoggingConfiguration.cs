using DM.Services.Core.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Filters;

namespace DM.Services.Core.Logging;

/// <summary>
/// Configuration of logging
/// </summary>
public static class LoggingConfiguration
{
    /// <summary>
    /// Register logger and add it to the service collection of the application
    /// </summary>
    public static IServiceCollection AddDmLogging(this IServiceCollection services,
        string applicationName, IConfigurationRoot configuration = null)
    {
        if (configuration == null)
        {
            configuration = ConfigurationFactory.Default;
        }

        var connectionStrings = new ConnectionStrings();
        configuration.GetSection(nameof(ConnectionStrings)).Bind(connectionStrings);

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .Enrich.FromLogContext()
            .Enrich.WithProperty("Application", applicationName)
            .Enrich.WithProperty("Environment", "Test")
            .WriteTo.Logger(lc => lc
                .Filter.ByExcluding(Matching.FromSource("Microsoft"))
                .WriteTo.Elasticsearch(
                    connectionStrings.Logs,
                    "dm_logstash-{0:yyyy.MM.dd}",
                    inlineFields: true))
            .WriteTo.Logger(lc => lc
                .WriteTo.Console())
            .CreateLogger();

        return services.AddLogging(b => b.AddSerilog());
    }
}