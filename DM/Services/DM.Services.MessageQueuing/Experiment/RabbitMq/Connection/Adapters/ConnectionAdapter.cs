using System;
using System.Threading;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace DM.Services.MessageQueuing.Experiment.RabbitMq.Connection.Adapters
{
    internal class ConnectionAdapter : IConnectionAdapter
    {
        private readonly IConnection connection;
        private readonly int channelsLimit;
        private readonly TimeSpan releaseTimeout;
        private readonly IChannelAdapterFactory channelAdapterFactory;
        private readonly ILogger logger;
        private readonly SemaphoreSlim semaphore;
        private bool disposed;

        public ConnectionAdapter(
            IConnection connection,
            int channelsLimit,
            TimeSpan releaseTimeout,
            IChannelAdapterFactory channelAdapterFactory,
            ILogger logger)
        {
            this.connection = connection;
            this.channelsLimit = channelsLimit;
            this.releaseTimeout = releaseTimeout;
            this.channelAdapterFactory = channelAdapterFactory;
            this.logger = logger;
            
            Id = Guid.NewGuid();
            semaphore = new SemaphoreSlim(channelsLimit, channelsLimit);
            connection.ConnectionShutdown += FireShutdownEvent;
        }
        
        public Guid Id { get; }
        public decimal BusinessRatio => 1 - (decimal)semaphore.CurrentCount / channelsLimit;

        public ConnectionBusinessStatus Status => BusinessRatio switch
        {
            >= 1M => ConnectionBusinessStatus.Full,
            >= 0.8M => ConnectionBusinessStatus.Busy,
            >= 0.4M => ConnectionBusinessStatus.Working,
            > 0 => ConnectionBusinessStatus.Free,
            _ => ConnectionBusinessStatus.Idle
        };

        public IChannelAdapter OpenChannel()
        {
            if (!semaphore.Wait(releaseTimeout))
            {
                throw new ConnectionChannelsExceededException(channelsLimit);
            }

            var channel = connection.CreateModel();
            channel.ModelShutdown += FreeChannel;
            logger.LogDebug(
                $"Открыт канал в соединении {{mqConnectionId}} ({channelsLimit - semaphore.CurrentCount}/{channelsLimit} - {BusinessRatio:P}, статус: {Status})",
                Id);
            return channelAdapterFactory.Create(channel, logger);
        }

        private void FreeChannel(object sender, ShutdownEventArgs e)
        {
            semaphore.Release();
            logger.LogDebug(
                $"Закрыт канал в соединении {{mqConnectionId}} ({channelsLimit - semaphore.CurrentCount}/{channelsLimit} - {BusinessRatio:P}, статус: {Status})",
                Id);
            var channel = (IModel)sender;
            channel.ModelShutdown -= FreeChannel;
        }

        private void FireShutdownEvent(object sender, ShutdownEventArgs e)
        {
            OnDisrupted?.Invoke(this, new ConnectionDisruptedEventArgs());
        }

        public event EventHandler<ConnectionDisruptedEventArgs> OnDisrupted;

        public void Dispose()
        {
            if (disposed)
            {
                return;
            }

            connection.ConnectionShutdown -= FireShutdownEvent;
            connection.Dispose();

            semaphore.Dispose();

            disposed = true;
        }
    }
}