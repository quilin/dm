using System.ComponentModel;
using MQExchangeType = RabbitMQ.Client.ExchangeType;

namespace DM.Services.MessageQueuing.Experiment.RabbitMq.Configuration
{
    /// <summary>
    /// Тип точки обмена
    /// </summary>
    public enum ExchangeType
    {
        /// <summary>
        /// Topic: сообщения можно помечать ключем роутинга, в зависимости от этого будет выбираться очередь, в которую отправится сообщение
        /// </summary>
        [Description(MQExchangeType.Topic)]
        Topic,

        /// <summary>
        /// Fanout: ключ роутинга будет игнорироваться и сообщение попадет во все привязанные очереди
        /// </summary>
        [Description(MQExchangeType.Fanout)]
        Fanout,

        /// <summary>
        /// Direct: привязанный ключ роутинга должен полностью совпадать c тем, что в сообщении
        /// </summary>
        [Description(MQExchangeType.Direct)]
        Direct,
    }
}