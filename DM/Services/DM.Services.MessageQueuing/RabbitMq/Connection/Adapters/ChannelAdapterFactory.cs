using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace DM.Services.MessageQueuing.RabbitMq.Connection.Adapters
{
    internal class ChannelAdapterFactory : IChannelAdapterFactory
    {
        public IChannelAdapter Create(IModel model, ILogger logger) => new ChannelAdapter(model, logger);
    }
}