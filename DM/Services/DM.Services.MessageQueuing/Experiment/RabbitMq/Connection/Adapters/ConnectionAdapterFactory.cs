using System;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace DM.Services.MessageQueuing.Experiment.RabbitMq.Connection.Adapters
{
    internal class ConnectionAdapterFactory : IConnectionAdapterFactory
    {
        private readonly IChannelAdapterFactory channelAdapterFactory;

        public ConnectionAdapterFactory(
            IChannelAdapterFactory channelAdapterFactory)
        {
            this.channelAdapterFactory = channelAdapterFactory;
        }

        public IConnectionAdapter Create(IConnection connection, int channelsLimit, TimeSpan releaseTimeout, ILogger logger) =>
            new ConnectionAdapter(connection, channelsLimit, releaseTimeout, channelAdapterFactory, logger);
    }
}