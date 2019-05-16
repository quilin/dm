using System;
using DM.Services.MessageQueuing.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace DM.Services.MessageQueuing.Consume
{
    /// <inheritdoc cref="IMessageConsumer{TMessage}" />
    public class MessageConsumer<TMessage> : IDisposable, IMessageConsumer<TMessage>
        where TMessage : class
    {
        private readonly IEventProcessorAdapter<TMessage> eventProcessorAdapter;
        private readonly IConnection connection;
        private readonly IModel channel;

        /// <inheritdoc />
        public MessageConsumer(
            IConnectionFactory connectionFactory,
            IEventProcessorAdapter<TMessage> eventProcessorAdapter)
        {
            this.eventProcessorAdapter = eventProcessorAdapter;
            connection = connectionFactory.CreateConnection();
            channel = connection.CreateModel();
        }

        /// <inheritdoc />
        public void Consume(MessageConsumeConfiguration configuration)
        {
            Ensure.Consume(channel, configuration);
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (_, args) => await eventProcessorAdapter.ProcessEvent(args, channel);
            channel.BasicConsume(consumer, configuration.QueueName);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            channel.Close();
            connection.Close();
        }
    }
}