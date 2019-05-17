using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using Serilog.Filters;

namespace DM.Services.Core.Logging
{
    /// <summary>
    /// Configuration of logging
    /// </summary>
    public static class LoggingConfiguration
    {
        /// <summary>
        /// Create and register logger for the application
        /// </summary>
        public static void Register()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Application", "DM.API")
                .Enrich.WithProperty("Environment", "Test")
                .WriteTo.Logger(lc => lc
                    .Filter.ByExcluding(Matching.FromSource("Microsoft"))
                    .WriteTo.Elasticsearch(
                        "http://localhost:9200",
                        "dm_logs-{0:yyyy.MM.dd}",
                        inlineFields: true))
                .WriteTo.Logger(lc => lc
                    .Filter.ByExcluding(x => x.Level == LogEventLevel.Debug)
                    .WriteTo.Console())
                .CreateLogger();
        }
        
        /// <summary>
        /// Register logger and add it to the service collection of the application
        /// </summary>
        public static IServiceCollection AddDmLogging(this IServiceCollection services)
        {
            Register();
            return services.AddLogging(b => b.AddSerilog());
        }
    }
}