using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Serilog;

namespace DM.Web.Classic
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            var webHostBuilder = WebHost.CreateDefaultBuilder(args)
                .UseSerilog()
                .UseKestrel(options => options.AllowSynchronousIO = true)
                .UseStartup<Startup>();

            // For heroku deployment, where only available port is defined in runtime by the environment variable
            var predefinedPort = Environment.GetEnvironmentVariable("PORT");
            return string.IsNullOrEmpty(predefinedPort)
                ? webHostBuilder
                : webHostBuilder.UseUrls($"http://*:{predefinedPort}");
        }
    }
}