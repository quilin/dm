using System;
using System.Threading;
using System.Threading.Tasks;
using DM.Services.MessageQueuing.RabbitMq.Configuration;
using DM.Services.MessageQueuing.RabbitMq.Connection;
using DM.Services.MessageQueuing.RabbitMq.Connection.Adapters;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace DM.Services.MessageQueuing.RabbitMq.Consuming
{
    internal class RabbitConsumer<TMessage, THandler> : IConsumer<TMessage>
        where THandler : IMessageHandler<TMessage>
    {
        private readonly RabbitConsumerParameters parameters;
        private readonly IMessageProcessor<TMessage, THandler> messageProcessor;
        private readonly IConnectionPool connectionPool;
        private readonly ILogger<RabbitConsumer<TMessage, THandler>> logger;

        private readonly object sync = new();
        private CountdownEvent countdownEvent;
        private CancellationTokenSource cancellationTokenSource;
        private bool running;

        private Lazy<ConsumerConnectionState> connectionAccessor;
        private readonly Func<IModel> channelAccessor;

        public RabbitConsumer(
            RabbitConsumerParameters parameters,
            IMessageProcessor<TMessage, THandler> messageProcessor,
            IConsumerConnectionPool connectionPool,
            ILogger<RabbitConsumer<TMessage, THandler>> logger)
        {
            this.parameters = parameters;
            this.messageProcessor = messageProcessor;
            this.connectionPool = connectionPool;
            this.logger = logger;

            channelAccessor = () => connectionAccessor?.Value.ChannelAdapter.GetChannel();
        }

        public void Start()
        {
            lock (sync)
            {
                if (running)
                {
                    return;
                }

                countdownEvent = new CountdownEvent(1);
                cancellationTokenSource = new CancellationTokenSource();

                connectionAccessor = CreateConnectionAccessor();
                parameters.DeclaredQueueName = Ensure.Consume(channelAccessor(), parameters).QueueName;
                Consume();

                running = true;
            }
        }

        public void Stop()
        {
            lock (sync)
            {
                if (!running || connectionAccessor is not { IsValueCreated: true })
                {
                    return;
                }

                var consumer = connectionAccessor.Value.Consumer;
                Task.Run(async () => await consumer.OnCancel()).GetAwaiter().GetResult();
                countdownEvent.Signal();
                cancellationTokenSource.Cancel();
                if (!countdownEvent.Wait(parameters.MaxProcessingAnticipation))
                {
                    logger.LogError("Произошла принудительная остановка работы обработчика сообщений. Часть сообщений может быть обработана некорректно!");
                }

                CloseCurrentConnection(connectionIsDisrupted: false);

                countdownEvent.Dispose();
                cancellationTokenSource.Dispose();
                running = false;
            }
        }

        public bool IsIdle()
        {
            lock (sync)
            {
                if (!running || countdownEvent.IsSet)
                {
                    return true;
                }

                using var channelAdapter = connectionPool.Get();
                var queueEmpty = channelAdapter.GetChannel().QueueDeclarePassive(parameters.QueueName).MessageCount == 0;
                var processorIdle = countdownEvent.CurrentCount == countdownEvent.InitialCount;

                return processorIdle && queueEmpty;
            }
        }

        private Lazy<ConsumerConnectionState> CreateConnectionAccessor() => new(() =>
        {
            var channelAdapter = connectionPool.Get();
            channelAdapter.OnDisrupted += Restore;

            var channel = channelAdapter.GetChannel();
            var consumer = new AsyncEventingBasicConsumer(channel);
            var currentCancellationTokenSource = cancellationTokenSource;

            async Task IncomingMessageHandler(object _, BasicDeliverEventArgs args)
            {
                try
                {
                    await messageProcessor.Process(
                        countdownEvent, channelAccessor, currentCancellationTokenSource, args, logger);
                }
                catch (Exception e)
                {
                    logger.LogCritical("Обработчик сообщения выбросил необработанное исключение", e);
                }
            }

            consumer.Received += IncomingMessageHandler;

            var consumerTag = $"{parameters.ConsumerTag}-{Guid.NewGuid()}";
            return new ConsumerConnectionState(IncomingMessageHandler, consumer, channelAdapter, consumerTag);
        }, LazyThreadSafetyMode.ExecutionAndPublication);

        private void CloseCurrentConnection(bool connectionIsDisrupted)
        {
            if (connectionAccessor is not { IsValueCreated: true })
            {
                return;
            }

            var connectionState = connectionAccessor.Value;

            connectionState.Consumer.Received -= connectionState.IncomingMessageHandler;
            connectionState.ChannelAdapter.OnDisrupted -= Restore;

            if (!connectionIsDisrupted)
            {
                connectionState.ChannelAdapter.GetChannel().BasicCancel(connectionState.ConsumerTag);
            }

            connectionState.ChannelAdapter.Dispose();
            connectionAccessor = null;
        }

        private void Restore(object sender, ConnectionDisruptedEventArgs args)
        {
            lock (sync)
            {
                logger.LogDebug("Прервалось соединение консюмера с брокером, пытаемся восстановить...");
                CloseCurrentConnection(connectionIsDisrupted: true);
                connectionAccessor = CreateConnectionAccessor();

                if (running)
                {
                    Consume();
                }
            }
        }

        private void Consume()
        {
            lock (sync)
            {
                var connectionState = connectionAccessor.Value;
                channelAccessor().BasicConsume(
                    parameters.DeclaredQueueName,
                    false,
                    connectionState.ConsumerTag,
                    connectionState.Consumer);
                running = true;
            }
        }

        public void Dispose() => Stop();
    }
}