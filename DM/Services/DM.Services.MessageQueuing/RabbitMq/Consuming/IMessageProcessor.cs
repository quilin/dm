using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace DM.Services.MessageQueuing.RabbitMq.Consuming
{
    internal interface IMessageProcessor<in TMessage, in THandler>
        where THandler : IMessageHandler<TMessage>
    {
        Task Process(CountdownEvent countdownEvent,
            Func<IModel> channelAccessor,
            CancellationTokenSource cancellationTokenSource,
            BasicDeliverEventArgs deliverArguments,
            ILogger logger);
    }
}