namespace DM.Services.MessageQueuing.Experiment.RabbitMq.Configuration
{
    /// <summary>
    /// Параметр очереди RMQ, отвечающий за максимальное количество сообщений,
    /// отдаваемых консюмеру до получения от него Ack/Nack
    /// </summary>
    public class PrefetchCount
    {
        internal PrefetchCount(ushort prefetchCount)
        {
            Value = prefetchCount;
        }

        /// <summary>
        /// Значение параметра
        /// </summary>
        public ushort Value { get; }
    }
}