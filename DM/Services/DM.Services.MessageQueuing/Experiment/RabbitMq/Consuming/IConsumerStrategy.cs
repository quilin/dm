using System.Threading;
using System.Threading.Tasks;
using RabbitMQ.Client.Events;

namespace DM.Services.MessageQueuing.Experiment.RabbitMq.Consuming
{
    internal interface IConsumerStrategy<out TMessage, out THandler>
        where THandler : IMessageHandler<TMessage>
    {
        Task<ProcessResult> Execute(BasicDeliverEventArgs deliverArguments, CancellationToken cancellationToken);
    }
}