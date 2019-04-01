using System;
using System.Text;
using System.Threading.Tasks;
using DM.Services.MessageQueuing.Configuration;
using DM.Services.MessageQueuing.Processing;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace DM.Services.MessageQueuing.Consume
{
    /// <inheritdoc cref="IMessageConsumer{TMessage}" />
    public class MessageConsumer<TMessage> : IDisposable, IMessageConsumer<TMessage>
        where TMessage : class
    {
        private readonly Func<IMessageProcessorWrapper<TMessage>> processorFactory;
        private readonly IConnection connection;
        private readonly IModel channel;

        /// <inheritdoc />
        public MessageConsumer(
            IConnectionFactory connectionFactory,
            Func<IMessageProcessorWrapper<TMessage>> processorFactory)
        {
            this.processorFactory = processorFactory;
            connection = connectionFactory.CreateConnection();
            channel = connection.CreateModel();
        }

        /// <inheritdoc />
        public void Consume(MessageConsumeConfiguration configuration)
        {
            channel.ExchangeDeclare(configuration.ExchangeName, "topic", true);
            channel.QueueDeclare(configuration.QueueName, true, false, false);
            foreach (var routingKey in configuration.RoutingKeys)
            {
                channel.QueueBind(configuration.QueueName, configuration.ExchangeName, routingKey);
            }

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (_, args) => await ConsumeMessage(args);
            channel.BasicConsume(consumer, configuration.QueueName);
        }

        private async Task ConsumeMessage(BasicDeliverEventArgs eventArgs)
        {
            var message = JsonConvert.DeserializeObject<TMessage>(Encoding.UTF8.GetString(eventArgs.Body));
            var processor = processorFactory();
            try
            {
                var result = await processor.ProcessWithCorrelation(message, eventArgs.BasicProperties.CorrelationId);
                switch (result)
                {
                    case ProcessResult.Success:
                        channel.BasicAck(eventArgs.DeliveryTag, false);
                        break;
                    case ProcessResult.RetryNeeded:
                        channel.BasicNack(eventArgs.DeliveryTag, false, true);
                        break;
                    case ProcessResult.Fail:
                        channel.BasicNack(eventArgs.DeliveryTag, false, false);
                        break;
                    default:
                        channel.BasicReject(eventArgs.DeliveryTag, false);
                        break;
                }
            }
            catch
            {
                channel.BasicNack(eventArgs.DeliveryTag, false, false);
            }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            channel.Close();
            connection.Close();
        }
    }
}