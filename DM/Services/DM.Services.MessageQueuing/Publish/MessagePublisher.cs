using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using DM.Services.Core.Implementation.CorrelationToken;
using DM.Services.MessageQueuing.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Framing;

namespace DM.Services.MessageQueuing.Publish
{
    /// <inheritdoc />
    public class MessagePublisher : IMessagePublisher
    {
        private readonly IConnectionFactory connectionFactory;
        private readonly ICorrelationTokenProvider correlationTokenProvider;

        /// <inheritdoc />
        public MessagePublisher(
            IConnectionFactory connectionFactory,
            ICorrelationTokenProvider correlationTokenProvider)
        {
            this.connectionFactory = connectionFactory;
            this.correlationTokenProvider = correlationTokenProvider;
        }

        /// <inheritdoc />
        public Task Publish<TMessage>(TMessage message, MessagePublishConfiguration configuration, string routingKey)
            where TMessage : class
        {
            using (var connection = connectionFactory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
                channel.BasicPublish(configuration.ExchangeName, routingKey,
                    new BasicProperties
                    {
                        Persistent = true,
                        ContentType = MediaTypeNames.Application.Json,
                        CorrelationId = correlationTokenProvider.Current.ToString()
                    }, body);
                return Task.CompletedTask;
            }
        }
    }
}