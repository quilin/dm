using System;
using System.Collections.Generic;
using DM.Services.MessageQueuing.Configuration;
using DM.Services.MessageQueuing.Consume;
using Microsoft.Extensions.DependencyInjection;

namespace DM.Services.Mail.Sender.Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("[🚴] Starting mail sender consumer");
            Console.WriteLine("[🔧] Configuring service provider");
            using (var serviceProvider = ContainerConfiguration.ConfigureProvider())
            {
                Console.WriteLine("[🐣] Creating consumer...");
                var messageConsumer = serviceProvider.GetService<IMessageConsumer<MailLetter>>();
                var configuration = new MessageConsumeConfiguration
                {
                    ExchangeName = "dm.mail.sending",
                    RoutingKeys = new[] {"#"},
                    QueueName = "dm.mail.sending",
                    Arguments = new Dictionary<string, object>
                    {
                        ["x-dead-letter-exchange"] = "dm.mail.unsent"
                    },
                    PrefetchCount = 1
                };
                messageConsumer.Consume(configuration);
                Console.WriteLine($"[👂] Consumer is listening to {configuration.QueueName} queue");
                Console.ReadLine();
            }
        }
    }
}