using DM.Services.MessageQueuing.RabbitMq.Connection.Adapters;

namespace DM.Services.MessageQueuing.RabbitMq.Connection
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