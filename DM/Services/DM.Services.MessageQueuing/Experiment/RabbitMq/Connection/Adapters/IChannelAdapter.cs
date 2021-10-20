using System;
using RabbitMQ.Client;

namespace DM.Services.MessageQueuing.Experiment.RabbitMq.Connection.Adapters
{
    internal interface IChannelAdapter : IDisposable
    {
        IModel GetChannel();
        event EventHandler<ConnectionDisruptedEventArgs> OnDisrupted;
    }
}