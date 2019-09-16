using System;
using DM.Services.MessageQueuing.Consume;
using DM.Services.MessageQueuing.Dto;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace DM.Services.Search.Consumer
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("[🚴] Starting search engine consumer");
            Console.WriteLine("[🔧] Configuring service provider");
            using (var serviceProvider = ContainerConfiguration.ConfigureProvider())
            {
                Console.WriteLine("[🐣] Creating consumer...");
                var messageConsumer = serviceProvider.GetService<IMessageConsumer<InvokedEvent>>();
                var configuration = serviceProvider.GetService<IOptions<DmEventConsumeConfiguration>>().Value;
                messageConsumer.Consume(configuration);
                Console.WriteLine($"[👂] Consumer is listening to {configuration.QueueName} queue");
                Console.ReadLine();
            }
        }
    }
}