using DM.Services.MessageQueuing.Configuration;

namespace DM.Services.MessageQueuing.Consume
{
    // ReSharper disable once UnusedTypeParameter
    /// <summary>
    /// Consumer for message of given type
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public interface IMessageConsumer<in TMessage> where TMessage : class
    {
        /// <summary>
        /// Start consuming
        /// </summary>
        /// <param name="configuration">MQ configuration</param>
        void Consume(MessageConsumeConfiguration configuration);
    }
}