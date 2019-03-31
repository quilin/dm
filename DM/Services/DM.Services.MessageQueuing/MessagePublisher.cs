using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using DM.Services.MessageQueuing.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Framing;

namespace DM.Services.MessageQueuing
{
    public class MessagePublisher : IMessagePublisher
    {
        private readonly IConnectionFactory connectionFactory;

        public MessagePublisher(
            IConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory;
        }

        public Task Publish<TMessage>(TMessage message, MessagePublishConfiguration configuration)
            where TMessage : class
        {
            using (var connection = connectionFactory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(configuration.ExchangeName, configuration.ExchangeType, true);
                channel.QueueDeclare(configuration.DurableQueueName, true);
                channel.QueueBind(configuration.DurableQueueName, configuration.ExchangeName, "#");

                var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
                channel.BasicPublish(configuration.ExchangeName, configuration.RoutingKey,
                    new BasicProperties
                    {
                        Persistent = true,
                        ContentType = MediaTypeNames.Application.Json
                    }, body);
                return Task.CompletedTask;
            }
        }
    }
}