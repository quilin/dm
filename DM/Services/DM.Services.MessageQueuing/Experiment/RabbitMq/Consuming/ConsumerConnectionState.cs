using DM.Services.MessageQueuing.Experiment.RabbitMq.Connection.Adapters;
using RabbitMQ.Client.Events;

namespace DM.Services.MessageQueuing.Experiment.RabbitMq.Consuming
{
    internal class ConsumerConnectionState
    {
        public ConsumerConnectionState(
            AsyncEventHandler<BasicDeliverEventArgs> incomingMessageHandler,
            AsyncEventingBasicConsumer consumer,
            IChannelAdapter channelAdapter,
            string consumerTag)
        {
            IncomingMessageHandler = incomingMessageHandler;
            Consumer = consumer;
            ChannelAdapter = channelAdapter;
            ConsumerTag = consumerTag;
        }

        public AsyncEventHandler<BasicDeliverEventArgs> IncomingMessageHandler { get; }
        public AsyncEventingBasicConsumer Consumer { get; }
        public IChannelAdapter ChannelAdapter { get; }
        public string ConsumerTag { get; }
    }
}