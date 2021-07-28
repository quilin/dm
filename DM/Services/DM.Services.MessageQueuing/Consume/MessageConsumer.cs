using System;
using DM.Services.Core.Implementation;
using DM.Services.MessageQueuing.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace DM.Services.MessageQueuing.Consume
{
    /// <inheritdoc cref="IMessageConsumer{TMessage}" />
    internal class MessageConsumer<TMessage> : IDisposable, IMessageConsumer<TMessage>
        where TMessage : class
    {
        private readonly IConnectionFactory connectionFactory;
        private readonly IGuidFactory guidFactory;
        private readonly IEventProcessorAdapter<TMessage> eventProcessorAdapter;
        private IConnection connection;
        private IModel channel;
        private string consumerTag;

        /// <inheritdoc />
        public MessageConsumer(
            IConnectionFactory connectionFactory,
            IGuidFactory guidFactory,
            IEventProcessorAdapter<TMessage> eventProcessorAdapter)
        {
            this.connectionFactory = connectionFactory;
            this.guidFactory = guidFactory;
            this.eventProcessorAdapter = eventProcessorAdapter;
        }

        /// <inheritdoc />
        public void Consume(MessageConsumeConfiguration configuration)
        {
            connection = connectionFactory.CreateConnection();
            channel = connection.CreateModel();
            Ensure.Consume(channel, configuration);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (_, args) => await eventProcessorAdapter.ProcessEvent(args, channel);

            var consumerTagBody = string.IsNullOrEmpty(configuration.ConsumerTag)
                ? GetType().Name
                : configuration.ConsumerTag;
            consumerTag = $"{consumerTagBody}.{guidFactory.Create()}";

            channel.BasicConsume(consumer, configuration.QueueName, false, consumerTag);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            channel?.BasicCancel(consumerTag);
            channel?.Close();
            connection?.Close();
        }
    }
}