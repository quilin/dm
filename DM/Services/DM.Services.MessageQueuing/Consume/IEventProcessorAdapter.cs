using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace DM.Services.MessageQueuing.Consume
{
    /// <summary>
    /// Adapter for incoming messages event consumer
    /// </summary>
    public interface IEventProcessorAdapter<in TMessage>
        where TMessage : class
    {
        /// <summary>
        /// Decode and process message and response to the MQ
        /// </summary>
        /// <param name="eventArgs">Incoming message information</param>
        /// <param name="channel">Channel to respond to</param>
        /// <returns></returns>
        Task ProcessEvent(BasicDeliverEventArgs eventArgs, IModel channel);
    }
}