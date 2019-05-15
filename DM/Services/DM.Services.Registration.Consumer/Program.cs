using System;
using DM.Services.MessageQueuing.Configuration;
using DM.Services.MessageQueuing.Consume;
using DM.Services.MessageQueuing.Dto;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Events;
using Serilog.Filters;

namespace DM.Services.Registration.Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("[🚴] Starting registration consumer");
            Console.WriteLine("[🔧] Configuring service provider");
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Application", "DM.Mail.Consumer")
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
            using (var serviceProvider = ContainerConfiguration.ConfigureProvider())
            {
                Console.WriteLine("[🐣] Creating consumer...");
                var messageConsumer = serviceProvider.GetService<IMessageConsumer<InvokedEvent>>();
                var configuration = serviceProvider.GetService<IOptions<MessageConsumeConfiguration>>().Value;
                messageConsumer.Consume(configuration);
                Console.WriteLine($"[👂] Consumer is listening to {configuration.QueueName} queue");
                Console.ReadLine();
            }
        }
    }
}