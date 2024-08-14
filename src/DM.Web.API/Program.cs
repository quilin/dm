using Autofac.Extensions.DependencyInjection;
using DM.Services.Core.Configuration;
using DM.Services.Core.Extensions;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace DM.Web.API;

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
        CreateWebHostBuilder(args)
            .WithDmConfiguration()
            .Build().Run();
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
            .UseSerilog()
            .ConfigureWebHostDefaults(webBuilder => webBuilder.UseDefault<Startup>());
    }
}