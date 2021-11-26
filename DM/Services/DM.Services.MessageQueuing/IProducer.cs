using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DM.Services.MessageQueuing
{
    /// <summary>
    /// Продюсер
    /// </summary>
    /// <typeparam name="TKey">Тип ключа</typeparam>
    /// <typeparam name="TMessage">Тип сообщения</typeparam>
    public interface IProducer<TKey, TMessage> : IDisposable
    {
        /// <summary>
        /// Отправить сообщение брокеру
        /// </summary>
        /// <param name="key">Ключ маршрутизации</param>
        /// <param name="message">Сообщение</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task Send(TKey key, TMessage message, CancellationToken cancellationToken);

        /// <summary>
        /// Отправить набор сообщений брокеру
        /// </summary>
        /// <param name="batch">Набор сообщений</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task Send(IEnumerable<(TKey key, TMessage message)> batch, CancellationToken cancellationToken);
    }
}