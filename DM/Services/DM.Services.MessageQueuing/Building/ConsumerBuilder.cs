using Autofac;
using DM.Services.MessageQueuing.RabbitMq.Configuration;
using DM.Services.MessageQueuing.RabbitMq.Consuming;

namespace DM.Services.MessageQueuing.Building
{
    internal class ConsumerBuilder<TMessage> : IConsumerBuilder<TMessage>
    {
        private readonly ILifetimeScope container;

        public ConsumerBuilder(
            ILifetimeScope container)
        {
            this.container = container;
        }

        public IConsumer<TMessage> BuildRabbit<THandler>(RabbitConsumerParameters parameters)
            where THandler : IMessageHandler<TMessage>
        {
            var consumerType = typeof(RabbitConsumer<,>).MakeGenericType(typeof(TMessage), typeof(THandler));
            var consumer = container.Resolve(consumerType,
                new TypedParameter(typeof(RabbitConsumerParameters), parameters));
            return (IConsumer<TMessage>)consumer;
        }
    }
}