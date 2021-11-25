using System;
using System.Collections.Generic;

namespace DM.Services.MessageQueuing.RabbitMq.Configuration
{
    /// <summary>
    /// Параметры консюмера RabbitMQ
    /// </summary>
    public class RabbitConsumerParameters
    {
        /// <inheritdoc />
        public RabbitConsumerParameters(
            string consumerTag,
            PrefetchCount processingOrder)
        {
            ConsumerTag = consumerTag;
            ProcessingOrder = processingOrder;
        }

        /// <summary>
        /// Имя точки обмена
        /// </summary>
        public string ExchangeName { get; set; }

        /// <summary>
        /// Тип точки обмена
        /// </summary>
        public ExchangeType ExchangeType { get; set; }

        /// <summary>
        /// Имя очереди
        /// </summary>
        public string QueueName { get; set; }

        /// <summary>
        /// Ключи маршрутизации для привязки очереди к точке обмена
        /// </summary>
        public IEnumerable<string> RoutingKeys { get; set; }

        /// <summary>
        /// Флаг эксклюзивности очереди
        /// Эксклюзивные очереди живут, пока жив их единственный консюмер и умирают вместе с ним и всеми своими сообщениями
        /// Хороший пример использования - очереди для уведомлений SignalR
        /// </summary>
        public bool Exclusive { get; set; }

        /// <summary>
        /// Флаг необходимости DLX для очереди, куда будут попадать необработанные или некорректно обработанные сообщения
        /// </summary>
        public string DeadLetterExchange { get; set; }

        /// <summary>
        /// Порядок обработки сообщений
        /// Инициализация через <see cref="ProcessingOrder" />
        /// </summary>
        public PrefetchCount ProcessingOrder { get; }

        /// <summary>
        /// Тег консюмера, человекочитаемый, будет "посолен" при открытии подключения
        /// </summary>
        public string ConsumerTag { get; }

        /// <summary>
        /// При отписке от очереди необходимо дождаться, пока довыполняются активные обработчики, иначе могут возникнуть непредвиденные ошибки,
        /// поэтому мы ждем какое-то время, и после этого прибиваем исполнение. Новые сообщения в это время не приходят.
        /// </summary>
        public TimeSpan MaxProcessingAnticipation { get; set; } = TimeSpan.FromSeconds(30);
    }
}