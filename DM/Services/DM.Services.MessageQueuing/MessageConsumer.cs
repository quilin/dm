using System;
using System.Text;
using System.Threading.Tasks;
using DM.Services.MessageQueuing.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace DM.Services.MessageQueuing
{
    public class MessageConsumer<TMessage> : IDisposable, IMessageConsumer<TMessage> where TMessage : class
    {
        private readonly Func<IMessageProcessor<TMessage>> processorFactory;
        private readonly IConnection connection;
        private readonly IModel channel;

        public MessageConsumer(
            IConnectionFactory connectionFactory,
            Func<IMessageProcessor<TMessage>> processorFactory)
        {
            this.processorFactory = processorFactory;
            connection = connectionFactory.CreateConnection();
            channel = connection.CreateModel();
        }

        public void Consume(MessageConsumeConfiguration configuration)
        {
            channel.ExchangeDeclare(configuration.ExchangeName, "topic", true);
            channel.QueueDeclare(configuration.QueueName, true, false, false);
            channel.QueueBind(configuration.QueueName, configuration.ExchangeName, configuration.RoutingKey);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (_, args) => await ConsumeMessage(args);
            channel.BasicConsume(consumer, configuration.QueueName);
        }

        private async Task ConsumeMessage(BasicDeliverEventArgs eventArgs)
        {
            var message = JsonConvert.DeserializeObject<TMessage>(Encoding.UTF8.GetString(eventArgs.Body));
            var processor = processorFactory();
            switch (await processor.Process(message))
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

        public void Dispose()
        {
            channel.Close();
            connection.Close();
        }
    }
}