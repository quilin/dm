using System;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace DM.Web.API
{
    /// <summary>
    /// Hosting
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Main
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// Create web host builder
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IHostBuilder CreateWebHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .UseKestrel(options => options.AllowSynchronousIO = true)
                        .UseSerilog()
                        .UseStartup<Startup>();

                    // For heroku deployment, where only available port is defined in runtime by the environment variable
                    var predefinedPort = Environment.GetEnvironmentVariable("PORT");
                    if (!string.IsNullOrEmpty(predefinedPort))
                    {
                        webBuilder.UseUrls($"http://*:{predefinedPort}");
                    }
                });
        }
    }
}