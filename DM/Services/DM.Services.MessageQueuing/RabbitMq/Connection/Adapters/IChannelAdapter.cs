using System;
using RabbitMQ.Client;

namespace DM.Services.MessageQueuing.RabbitMq.Connection.Adapters
{
    internal interface IChannelAdapter : IDisposable
    {
        IModel GetChannel();
        event EventHandler<ConnectionDisruptedEventArgs> OnDisrupted;
    }
}