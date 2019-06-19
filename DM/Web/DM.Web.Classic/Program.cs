using DM.Services.Core.Logging;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Serilog;

namespace DM.Web.Classic
{
    public class Program
    {
        public static void Main(string[] args)
        {
            LoggingConfiguration.Register("DM.Classic");
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseLibuv()
                .UseSerilog()
                .UseStartup<Startup>();
    }
}