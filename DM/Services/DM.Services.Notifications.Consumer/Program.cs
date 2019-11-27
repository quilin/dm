using System;
using DM.Services.MessageQueuing.Configuration;
using DM.Services.MessageQueuing.Consume;
using DM.Services.MessageQueuing.Dto;
using DM.Services.MessageQueuing.Publish;
using Microsoft.Extensions.DependencyInjection;

namespace DM.Services.Notifications.Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("[🚴] Starting notifications generator consumer");
            Console.WriteLine("[🔧] Configuring service provider");
            using (var serviceProvider = ContainerConfiguration.ConfigureProvider())
            {
                Console.WriteLine("[🐣] Creating consumer...");
                var messageConsumer = serviceProvider.GetService<IMessageConsumer<InvokedEvent>>();
                var configuration = new MessageConsumeConfiguration
                {
                    ExchangeName = InvokedEventsTransport.ExchangeName,
                    RoutingKeys = new string[0],
                    QueueName = "dm.notifications.sending"
                };
                messageConsumer.Consume(configuration);
                Console.WriteLine($"[👂] Consumer is listening to {configuration.QueueName} queue");
                Console.ReadLine();
            }
        }
    }
}