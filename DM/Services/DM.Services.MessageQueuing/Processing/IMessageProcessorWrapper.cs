using System.Threading.Tasks;

namespace DM.Services.MessageQueuing.Processing
{
    /// <summary>
    /// Wrapper for <see cref="IMessageProcessor{TMessage}"/> to append correlation token
    /// </summary>
    /// <typeparam name="TMessage">Received message</typeparam>
    public interface IMessageProcessorWrapper<in TMessage>
    {
        /// <summary>
        /// Save correlation token and proceed with message processing
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="correlationToken">Correlation token</param>
        /// <returns>Process result</returns>
        Task<ProcessResult> ProcessWithCorrelation(TMessage message, string correlationToken);
    }
}