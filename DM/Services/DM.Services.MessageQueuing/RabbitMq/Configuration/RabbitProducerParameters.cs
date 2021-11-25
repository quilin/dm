using System;

namespace DM.Services.MessageQueuing.RabbitMq.Configuration
{
    /// <summary>
    /// Параметры продюсера RabbitMQ
    /// </summary>
    public class RabbitProducerParameters
    {
        /// <summary>
        /// Имя точки обмена
        /// </summary>
        public string ExchangeName { get; set; }

        /// <summary>
        /// Тип точки обмена
        /// </summary>
        public ExchangeType ExchangeType { get; set; }

        /// <summary>
        /// Количество попыток отправки сообщения
        /// </summary>
        public int RetryCount { get; set; } = 5;

        /// <summary>
        /// Время ожидания подтверждения получения сообщения от брокера
        /// Мы не ждем ответа от обработчиков, но ждем, что RMQ получил и смог маршрутизировать сообщени
        /// </summary>
        public TimeSpan? PublishingTimeout { get; set; } = TimeSpan.FromSeconds(10);
    }
}