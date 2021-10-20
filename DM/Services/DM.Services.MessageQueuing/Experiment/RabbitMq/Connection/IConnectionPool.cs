using DM.Services.MessageQueuing.Experiment.RabbitMq.Connection.Adapters;

namespace DM.Services.MessageQueuing.Experiment.RabbitMq.Connection
{
    internal interface IConnectionPool
    {
        IChannelAdapter Get();
    }

    internal interface IConsumerConnectionPool : IConnectionPool
    {
    }

    internal interface IProducerConnectionPool : IConnectionPool
    {
    }
}