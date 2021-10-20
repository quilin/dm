using System;

namespace DM.Services.MessageQueuing.Experiment.RabbitMq.Configuration
{
    /// <summary>
    /// Построитель параллельной обработки сообщений из очереди RMQ
    /// </summary>
    public class ParallelProcessing
    {
        /// <summary>
        /// С максимальной степенью параллелизма
        /// </summary>
        /// <param name="maximumDegree">Максимальная степень параллелизма</param>
        /// <returns></returns>
        public PrefetchCount WithMaximumDegree(ushort maximumDegree)
        {
            if (maximumDegree <= 1)
            {
                throw new ArgumentOutOfRangeException(nameof(maximumDegree), maximumDegree,
                    "Минимальная степень параллелизма - 2. Если вы хотите использовать степень 1 - пользуйтесь ProcessingOrder.Sequential");
            }

            return new PrefetchCount(maximumDegree);
        }
    }
}