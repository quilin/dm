using System.Threading.Tasks;

namespace DM.Services.MessageQueuing.Processing
{
    /// <summary>
    /// Processes received message
    /// </summary>
    /// <typeparam name="TMessage">Type of message</typeparam>
    public interface IMessageProcessor<in TMessage>
    {
        /// <summary>
        /// Process received message
        /// </summary>
        /// <param name="message"></param>
        /// <returns>Process result</returns>
        Task<ProcessResult> Process(TMessage message);
    }
}