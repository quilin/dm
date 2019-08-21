using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace DM.Services.Core.Configuration
{
    /// <summary>
    /// Generates configuration from settings file and 
    /// </summary>
    public static class ConfigurationFactory
    {
        private static readonly Lazy<IConfigurationRoot> Factory = new Lazy<IConfigurationRoot>(() =>
            new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true)
                .AddEnvironmentVariables("DM_")
                .Build());

        /// <summary>
        /// Generate basic configuration
        /// </summary>
        /// <returns></returns>
        public static IConfigurationRoot Default => Factory.Value;
    }
}