using System;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace DM.Services.MessageQueuing.Experiment.RabbitMq.Connection.Adapters
{
    internal class ChannelAdapter : IChannelAdapter
    {
        private const ushort ForceTerminationCode = 320;
        private const string ForceTerminationText = "CONNECTION_FORCED - Closed via management plugin";

        private readonly IModel model;
        private readonly ILogger logger;

        public ChannelAdapter(
            IModel model,
            ILogger logger)
        {
            this.model = model;
            this.logger = logger;
            model.ModelShutdown += ResolveShutdown;
        }

        private void ResolveShutdown(object sender, ShutdownEventArgs e)
        {
            if (e.Initiator != ShutdownInitiator.Peer ||
                e.ReplyCode == ForceTerminationCode && e.ReplyText == ForceTerminationText)
            {
                logger.LogDebug($"Канал разорван со стороны {e.Initiator} (причина: {e.ReplyCode} - {e.ReplyText})");
                return;
            }
            
            OnDisrupted?.Invoke(this, new ConnectionDisruptedEventArgs());
        }

        public IModel GetChannel() => model;

        public event EventHandler<ConnectionDisruptedEventArgs> OnDisrupted;

        public void Dispose()
        {
            model.ModelShutdown -= ResolveShutdown;
            model.Close();
            model.Dispose();
        }
    }
}