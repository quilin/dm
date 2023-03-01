using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace DM.Services.Core.Configuration;

/// <summary>
/// Extensions for quick environment variables configuration provider setup
/// </summary>
public static class WebHostBuilderExtensions
{
    /// <summary>
    /// Enrich web host builder with DM configuration sources
    /// </summary>
    /// <param name="builder">Web host builder</param>
    /// <returns>Builder itself for chaining</returns>
    public static IHostBuilder WithDmConfiguration(this IHostBuilder builder) =>
        builder.ConfigureAppConfiguration(configuration => configuration.AddEnvironmentVariables("DM_"));
}