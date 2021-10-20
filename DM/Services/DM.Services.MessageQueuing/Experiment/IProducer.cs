using System;
using System.Threading;
using System.Threading.Tasks;

namespace DM.Services.MessageQueuing.Experiment
{
    /// <summary>
    /// Продюсер
    /// </summary>
    /// <typeparam name="TKey">Тип ключа</typeparam>
    /// <typeparam name="TMessage">Тип сообщения</typeparam>
    public interface IProducer<in TKey, in TMessage> : IDisposable
    {
        /// <summary>
        /// Отправить сообщение брокеру
        /// </summary>
        /// <param name="key">Ключ маршрутизации</param>
        /// <param name="message">Сообщение</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task Send(TKey key, TMessage message, CancellationToken cancellationToken);
    }
}