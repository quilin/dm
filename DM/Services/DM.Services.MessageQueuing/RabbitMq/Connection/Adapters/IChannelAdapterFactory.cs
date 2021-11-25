using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace DM.Services.MessageQueuing.RabbitMq.Connection.Adapters
{
    internal interface IChannelAdapterFactory
    {
        IChannelAdapter Create(IModel model, ILogger logger);
    }
}