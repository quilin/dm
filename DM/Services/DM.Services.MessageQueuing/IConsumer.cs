using System;

namespace DM.Services.MessageQueuing
{
    /// <summary>
    /// Консюмер
    /// </summary>
    /// <typeparam name="TMessage">Тип входящих сообщений</typeparam>
    public interface IConsumer<out TMessage> : IDisposable
    {
        /// <summary>
        /// Возобновить принятие сообщений от брокера
        /// </summary>
        void Start();

        /// <summary>
        /// Приостановить принятие сообщение от брокера
        /// </summary>
        void Stop();

        /// <summary>
        /// Консюмер работает вхолостую
        /// </summary>
        /// <returns></returns>
        bool IsIdle();
    }
}