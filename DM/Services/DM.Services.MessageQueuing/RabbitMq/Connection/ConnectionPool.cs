using System;
using System.Collections.Generic;
using System.Linq;
using DM.Services.MessageQueuing.RabbitMq.Connection.Adapters;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using RabbitMQ.Client;

namespace DM.Services.MessageQueuing.RabbitMq.Connection
{
    internal class ConnectionPool : IProducerConnectionPool, IConsumerConnectionPool, IDisposable
    {
        private const int PoolSize = 16;
        private const int ChannelsLimit = 128;
        private static readonly TimeSpan ReleaseTimeout = TimeSpan.FromSeconds(30);
        
        private readonly ICollection<IConnectionAdapter> connections = new List<IConnectionAdapter>(PoolSize);
        private readonly IConnectionFactory connectionFactory;
        private readonly IConnectionAdapterFactory connectionAdapterFactory;
        private readonly ILogger<ConnectionPool> logger;
        private readonly RetryPolicy connectionRetryPolicy;

        private bool disposed;

        public ConnectionPool(
            IConnectionFactory connectionFactory,
            IConnectionAdapterFactory connectionAdapterFactory,
            ILogger<ConnectionPool> logger)
        {
            this.connectionFactory = connectionFactory;
            this.connectionAdapterFactory = connectionAdapterFactory;
            this.logger = logger;

            connectionRetryPolicy = Policy.Handle<Exception>()
                .WaitAndRetry(10, attempt => TimeSpan.FromSeconds(1 << attempt),
                    (e, interval) => logger.LogWarning($"Не удалось открыть соединение, повторная попытка через {interval}", e));
        }
        
        public IChannelAdapter Get()
        {
            lock (connections)
            {
                var currentConnections = connections
                    .GroupBy(c => c.Status)
                    .ToDictionary(g => g.Key, g => g.OrderBy(c => c.BusinessRatio).ToArray());

                if (currentConnections.TryGetValue(ConnectionBusinessStatus.Idle, out var idleConnections))
                {
                    if (idleConnections.Length > 1)
                    {
                        logger.LogDebug($"В пуле найдены неиспользуемые подключения, удаляем {idleConnections.Length - 1} из них");
                    }

                    foreach (var connection in idleConnections.Skip(1))
                    {
                        DisposeConnection(connection);
                    }

                    return idleConnections.First().OpenChannel();
                }

                if (currentConnections.TryGetValue(ConnectionBusinessStatus.Working, out var workingConnections))
                {
                    return workingConnections.First().OpenChannel();
                }

                if (currentConnections.TryGetValue(ConnectionBusinessStatus.Free, out var freeConnections))
                {
                    return freeConnections.First().OpenChannel();
                }

                if (connections.Count < PoolSize)
                {
                    var connectionAdapter = CreateConnection();
                    connectionAdapter.OnDisrupted += RemoveAndDisposeConnection;
                    connections.Add(connectionAdapter);
                    logger.LogDebug($"Создано новое соединение {{mqConnectionId}}, общее количество соединений в пуле: {connections.Count}",
                        connectionAdapter.Id);
                    return connectionAdapter.OpenChannel();
                }

                if (currentConnections.TryGetValue(ConnectionBusinessStatus.Busy, out var busyConnections))
                {
                    return busyConnections.First().OpenChannel();
                }
                
                logger.LogCritical("Переполнен пул подключений к RMQ! Новое подключение не может быть создано. Состояние пула: {mqConnectionPoolState}",
                    new
                    {
                        PoolSize,
                        ActualConnectionsCount = connections.Count,
                        ChannelsLimit,
                        ConnectionBusinessRations = connections.Select(c => c.BusinessRatio).ToArray()
                    });
                throw new ConnectionPoolOverflowException();
            }
        }

        private IConnectionAdapter CreateConnection() => connectionRetryPolicy.Execute(() =>
            connectionAdapterFactory.Create(connectionFactory.CreateConnection(), ChannelsLimit, ReleaseTimeout, logger));

        private void RemoveAndDisposeConnection(object sender, ConnectionDisruptedEventArgs args)
        {
            lock (connections)
            {
                var connectionAdapter = (IConnectionAdapter) sender;
                DisposeConnection(connectionAdapter);
                connections.Remove(connectionAdapter);
            }
        }

        private void DisposeConnection(IConnectionAdapter connectionAdapter)
        {
            lock (connections)
            {
                logger.LogDebug("Соединение {mqConnectionId} разорвано. Исключаем его из пула.", connectionAdapter.Id);
                connectionAdapter.OnDisrupted -= RemoveAndDisposeConnection;
                connectionAdapter.Dispose();
            }
        }

        public void Dispose()
        {
            lock (connections)
            {
                if (disposed)
                {
                    return;
                }

                foreach (var connection in connections)
                {
                    DisposeConnection(connection);
                }

                connections.Clear();
                disposed = true;
            }
        }
    }
}