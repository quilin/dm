using DM.Services.MessageQueuing.RabbitMq.Configuration;

namespace DM.Services.MessageQueuing.Building
{
    /// <summary>
    /// Построитель продюсера
    /// </summary>
    public interface IProducerBuilder<TKey, TMessage>
    {
        /// <summary>
        /// Построить продюсер для RabbitMQ
        /// </summary>
        /// <param name="parameters">Параметры продюсера</param>
        /// <returns></returns>
        IProducer<TKey, TMessage> BuildRabbit(RabbitProducerParameters parameters);
    }
}