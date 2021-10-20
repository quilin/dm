using System.Threading;
using System.Threading.Tasks;

namespace DM.Services.MessageQueuing.Experiment
{
    /// <summary>
    /// Обработчик сообщения от брокера
    /// </summary>
    /// <typeparam name="TMessage">Тип входящего сообщения</typeparam>
    public interface IMessageHandler<in TMessage>
    {
        /// <summary>
        /// Обработать входящее сообщение
        /// </summary>
        /// <param name="message">Входящее сообщение</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Результат обработки</returns>
        Task<ProcessResult> Handle(TMessage message, CancellationToken cancellationToken);
    }
}