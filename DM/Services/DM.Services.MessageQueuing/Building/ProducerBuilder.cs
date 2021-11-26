using Autofac;
using DM.Services.MessageQueuing.RabbitMq.Configuration;
using DM.Services.MessageQueuing.RabbitMq.Producing;

namespace DM.Services.MessageQueuing.Building
{
    internal class ProducerBuilder<TKey, TMessage> : IProducerBuilder<TKey, TMessage>
    {
        private readonly ILifetimeScope container;

        public ProducerBuilder(
            ILifetimeScope container)
        {
            this.container = container;
        }

        public IProducer<TKey, TMessage> BuildRabbit(RabbitProducerParameters parameters)
        {
            var producerType = typeof(RabbitProducer<>).MakeGenericType(typeof(TMessage));
            var producer = container.Resolve(producerType,
                new TypedParameter(typeof(RabbitProducerParameters), parameters));
            return (IProducer<TKey, TMessage>)producer;
        }
    }
}