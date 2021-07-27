using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Text.Json;
using System.Threading.Tasks;
using DM.Services.Core.Implementation.CorrelationToken;
using DM.Services.MessageQueuing.Configuration;
using DM.Services.MessageQueuing.Consume;
using RabbitMQ.Client;

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
            using var connection = connectionFactory.CreateConnection();
            using var channel = connection.CreateModel();

            Ensure.Publish(channel, configuration);

            var basicProperties = channel.CreateBasicProperties();
            basicProperties.Persistent = true;
            basicProperties.ContentType = MediaTypeNames.Application.Json;
            basicProperties.CorrelationId = correlationTokenProvider.Current.ToString();

            var bodyBytes = JsonSerializer.SerializeToUtf8Bytes(message, SerializationSettings.ForMessage);
            var body = new ReadOnlyMemory<byte>(bodyBytes);
            channel.BasicPublish(configuration.ExchangeName, routingKey, basicProperties, body);
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task Publish<TMessage>(IEnumerable<(TMessage message, string routingKey)> messages,
            MessagePublishConfiguration configuration)
            where TMessage : class
        {
            using var connection = connectionFactory.CreateConnection();
            using var channel = connection.CreateModel();

            Ensure.Publish(channel, configuration);
            var basicPublishBatch = channel.CreateBasicPublishBatch();
            foreach (var (message, routingKey) in messages)
            {
                var bodyBytes = JsonSerializer.SerializeToUtf8Bytes(message, SerializationSettings.ForMessage);
                var body = new ReadOnlyMemory<byte>(bodyBytes);
                var basicProperties = channel.CreateBasicProperties();
                basicProperties.Persistent = true;
                basicProperties.ContentType = MediaTypeNames.Application.Json;
                basicProperties.CorrelationId = correlationTokenProvider.Current.ToString();
                basicPublishBatch.Add(configuration.ExchangeName, routingKey, false, basicProperties, body);
            }

            basicPublishBatch.Publish();

            return Task.CompletedTask;
        }
    }
}