﻿using Autofac.Extensions.DependencyInjection;
using DM.Services.Core.Extensions;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace DM.Services.Search.Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// Create web host builder
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IHostBuilder CreateWebHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder => webBuilder
                    .UseDefaultGrpc<Startup>());
    }
}