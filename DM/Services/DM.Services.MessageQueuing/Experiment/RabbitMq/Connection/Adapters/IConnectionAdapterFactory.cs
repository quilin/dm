using System;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace DM.Services.MessageQueuing.Experiment.RabbitMq.Connection.Adapters
{
    internal interface IConnectionAdapterFactory
    {
        IConnectionAdapter Create(
            IConnection connection,
            int channelsLimit,
            TimeSpan releaseTimeout,
            ILogger logger);
    }
}