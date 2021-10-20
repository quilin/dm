namespace DM.Services.MessageQueuing.Experiment.RabbitMq.Configuration
{
    /// <summary>
    /// Порядок обработки сообщений из очереди
    /// </summary>
    public static class ProcessingOrder
    {
        /// <summary>
        /// Последовательная обработка
        /// </summary>
        public static PrefetchCount Sequential => new PrefetchCount(1);

        /// <summary>
        /// Параллельная обработка
        /// </summary>
        public static ParallelProcessing Parallel => new ParallelProcessing();

        /// <summary>
        /// Клиент не влияет на QoS
        /// </summary>
        public static readonly PrefetchCount Unmanaged = new PrefetchCount(0);
    }
}