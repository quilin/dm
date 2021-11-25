using DM.Services.MessageQueuing.RabbitMq.Configuration;

namespace DM.Services.MessageQueuing.Building
{
    /// <summary>
    /// Построитель консюмера
    /// </summary>
    /// <typeparam name="TMessage">Тип входящего сообщения</typeparam>
    public interface IConsumerBuilder<out TMessage>
    {
        /// <summary>
        /// Построить консюмер RabbitMQ
        /// </summary>
        /// <param name="parameters">Параметры консюмера</param>
        /// <typeparam name="THandler">Обработчик сообщений</typeparam>
        /// <returns></returns>
        IConsumer<TMessage> BuildRabbit<THandler>(RabbitConsumerParameters parameters)
            where THandler : IMessageHandler<TMessage>;
    }
}