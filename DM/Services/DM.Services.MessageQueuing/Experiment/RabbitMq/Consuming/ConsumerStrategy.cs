using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using RabbitMQ.Client.Events;

namespace DM.Services.MessageQueuing.Experiment.RabbitMq.Consuming
{
    internal class ConsumerStrategy<TMessage, THandler> : IConsumerStrategy<TMessage, THandler>
        where THandler : IMessageHandler<TMessage>
    {
        private readonly THandler handler;

        public ConsumerStrategy(
            THandler handler)
        {
            this.handler = handler;
        }
        
        public async Task<ProcessResult> Execute(BasicDeliverEventArgs deliverArguments, CancellationToken cancellationToken)
        {
            var message = JsonSerializer.Deserialize<TMessage>(deliverArguments.Body.ToArray());
            return await handler.Handle(message, cancellationToken);
        }
    }
}