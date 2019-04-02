using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Serilog;
using Serilog.Filters;

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
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Application", "DM.API")
                .Enrich.WithProperty("Environment", "Test")
                .WriteTo.Logger(lc => lc
//                    .Filter.ByExcluding(Matching.FromSource("Microsoft"))
                    .WriteTo.Elasticsearch("localhost:9200", "test_{0}"))
                .CreateLogger();
            CreateWebHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// Create web host builder
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseLibuv()
                .UseSerilog()
                .UseStartup<Startup>();
    }
}