using System.Threading.Tasks;
using DM.Services.MessageQueuing.Configuration;

namespace DM.Services.MessageQueuing.Publish
{
    /// <summary>
    /// MQ publisher
    /// </summary>
    public interface IMessagePublisher
    {
        /// <summary>
        /// Publish message of given type
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="configuration">Publish configuration</param>
        /// <param name="routingKey">Routing key</param>
        /// <typeparam name="TMessage">Message</typeparam>
        /// <returns></returns>
        Task Publish<TMessage>(TMessage message, MessagePublishConfiguration configuration, string routingKey)
            where TMessage : class;
    }
}