using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using DM.Services.Core.Implementation.CorrelationToken;
using DM.Services.MessageQueuing.RabbitMq.Configuration;
using DM.Services.MessageQueuing.RabbitMq.Connection;
using DM.Services.MessageQueuing.RabbitMq.Connection.Adapters;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using RabbitMQ.Client;

namespace DM.Services.MessageQueuing.RabbitMq.Producing
{
    internal class RabbitProducer<TMessage> : IProducer<string, TMessage>
    {
        private readonly RabbitProducerParameters parameters;
        private readonly IConnectionPool connectionPool;
        private readonly ICorrelationTokenProvider correlationTokenProvider;
        private readonly ILogger<RabbitProducer<TMessage>> logger;
        private readonly RetryPolicy sendRetryPolicy;

        private Lazy<IChannelAdapter> channelAccessor;

        public RabbitProducer(
            RabbitProducerParameters parameters,
            IProducerConnectionPool connectionPool,
            ICorrelationTokenProvider correlationTokenProvider,
            ILogger<RabbitProducer<TMessage>> logger)
        {
            this.parameters = parameters;
            this.connectionPool = connectionPool;
            this.correlationTokenProvider = correlationTokenProvider;
            this.logger = logger;

            channelAccessor = CreateChannelAccessor();
            sendRetryPolicy = Policy.Handle<Exception>()
                .WaitAndRetry(parameters.RetryCount, attempt => TimeSpan.FromSeconds(1 << attempt),
                    (e, interval) =>
                        logger.LogWarning(e, $"Не удалось отправить сообщение, повторная попытка через {interval}"));
        }

        private Lazy<IChannelAdapter> CreateChannelAccessor() => new(() =>
        {
            var channelAdapter = connectionPool.Get();
            channelAdapter.OnDisrupted += Restore;
            Ensure.Produce(channelAdapter.GetChannel(), parameters);
            return channelAdapter;
        }, LazyThreadSafetyMode.ExecutionAndPublication);

        private void CloseCurrentChannel()
        {
            if (!channelAccessor.IsValueCreated)
            {
                return;
            }

            var channelAdapter = channelAccessor.Value;
            channelAdapter.OnDisrupted -= Restore;
            channelAdapter.Dispose();
        }

        private void Restore(object sender, ConnectionDisruptedEventArgs e)
        {
            logger.LogDebug("Прервалось соединение продюсера с броером, пытаемся восстановить...");
            CloseCurrentChannel();
            channelAccessor = CreateChannelAccessor();
        }

        public Task Send(string key, TMessage message, CancellationToken cancellationToken)
        {
            var correlationId = correlationTokenProvider.Current;

            var sendResult = sendRetryPolicy.ExecuteAndCapture(_ =>
            {
                var channelAdapter = channelAccessor.Value;
                var channel = channelAdapter.GetChannel();

                var (properties, body) = PrepareMessagePart(channel, correlationId, message);

                var waitForConfirms = parameters.PublishingTimeout.HasValue;
                if (waitForConfirms)
                {
                    channel.ConfirmSelect();
                }

                channel.BasicPublish(parameters.ExchangeName, key, true, properties, body);

                if (waitForConfirms)
                {
                    channel.WaitForConfirmsOrDie(parameters.PublishingTimeout.Value);
                }
            }, cancellationToken);

            if (sendResult.Outcome == OutcomeType.Successful)
            {
                return Task.CompletedTask;
            }

            logger.LogError(sendResult.FinalException, "Не удалось отправить сообщение {message}", message);
            throw sendResult.FinalException;
        }

        public Task Send(IEnumerable<(string key, TMessage message)> batch, CancellationToken cancellationToken)
        {
            var correlationId = correlationTokenProvider.Current;

            var sendResult = sendRetryPolicy.ExecuteAndCapture(_ =>
            {
                var channelAdapter = channelAccessor.Value;
                var channel = channelAdapter.GetChannel();

                var basicPublishBatch = channel.CreateBasicPublishBatch();
                foreach (var (routingKey, message) in batch)
                {
                    var (properties, body) = PrepareMessagePart(channel, correlationId, message);
                    basicPublishBatch.Add(parameters.ExchangeName, routingKey, true, properties, body);
                }

                var waitForConfirms = parameters.PublishingTimeout.HasValue;
                if (waitForConfirms)
                {
                    channel.ConfirmSelect();
                }

                basicPublishBatch.Publish();

                if (waitForConfirms)
                {
                    channel.WaitForConfirmsOrDie(parameters.PublishingTimeout.Value);
                }
            }, cancellationToken);

            if (sendResult.Outcome == OutcomeType.Successful)
            {
                return Task.CompletedTask;
            }

            logger.LogError(sendResult.FinalException, "Не удалось отправить пакет сообщений");
            throw sendResult.FinalException;
        }

        private static (IBasicProperties properties, ReadOnlyMemory<byte> body) PrepareMessagePart(
            IModel channel, Guid correlationId, TMessage message)
        {
            var basicProperties = channel.CreateBasicProperties();
            basicProperties.CorrelationId = correlationId.ToString();
            basicProperties.Persistent = true;
            basicProperties.Type = typeof(TMessage).FullName;
            var body = new ReadOnlyMemory<byte>(
                JsonSerializer.SerializeToUtf8Bytes(message, SerializationSettings.ForMessage));
            return (basicProperties, body);
        }

        public void Dispose() => CloseCurrentChannel();
    }
}