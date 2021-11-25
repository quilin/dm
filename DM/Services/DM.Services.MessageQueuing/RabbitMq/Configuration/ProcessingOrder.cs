namespace DM.Services.MessageQueuing.RabbitMq.Configuration
{
    /// <summary>
    /// Порядок обработки сообщений из очереди
    /// </summary>
    public static class ProcessingOrder
    {
        /// <summary>
        /// Последовательная обработка
        /// </summary>
        public static PrefetchCount Sequential => new(1);

        /// <summary>
        /// Параллельная обработка
        /// </summary>
        public static ParallelProcessing Parallel => new();

        /// <summary>
        /// Клиент не влияет на QoS
        /// </summary>
        public static readonly PrefetchCount Unmanaged = new(0);
    }
}