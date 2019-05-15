using System;
using DM.Services.MessageQueuing.Configuration;
using DM.Services.MessageQueuing.Consume;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

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
                var configuration = serviceProvider.GetService<IOptions<MessageConsumeConfiguration>>().Value;
                messageConsumer.Consume(configuration);
                Console.WriteLine($"[👂] Consumer is listening to {configuration.QueueName} queue");
                Console.ReadLine();
            }
        }
    }
}