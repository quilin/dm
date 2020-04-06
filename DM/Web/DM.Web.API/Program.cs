using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Serilog;

namespace DM.Web.API
{
    /// <summary>
    /// Hosting
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args)
                .Build()
                .Run();
        }

        /// <summary>
        /// Create web host builder
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            var webHostBuilder = WebHost.CreateDefaultBuilder(args)
                .UseLibuv()
                .UseSerilog()
                .UseStartup<Startup>();

            // For heroku deployment, where only available port is defined in runtime by the environment variable
            var predefinedPort = Environment.GetEnvironmentVariable("PORT");
            return string.IsNullOrEmpty(predefinedPort)
                ? webHostBuilder
                : webHostBuilder.UseUrls($"http://*:{predefinedPort}");
        }
    }
}