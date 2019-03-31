using System;
using System.Threading.Tasks;
using DM.Services.DataAccess.Eventing;
using DM.Services.MessageQueuing;
using DM.Services.MessageQueuing.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace DM.Services.SearchEngine.Indexing
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Running search engine consumer...");
            Console.WriteLine("Configuring service provider...");
            using (var serviceProvider = ContainerConfiguration.ConfigureProvider())
            {
                Console.WriteLine("Creating consumer...");
                var messageConsumer = serviceProvider.GetService<IMessageConsumer<InvokedEvent>>();
                var configuration = serviceProvider.GetService<IOptions<MessageConsumeConfiguration>>().Value;
                messageConsumer.Consume(configuration);
                Console.WriteLine($"Consumer is listening to MQ {configuration.QueueName}");
                Console.ReadLine();
            }
        }
    }
}