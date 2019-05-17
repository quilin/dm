using RabbitMQ.Client;

namespace DM.Services.MessageQueuing.Configuration
{
    /// <summary>
    /// Connection configuration guarantor
    /// </summary>
    public static class Ensure
    {
        /// <summary>
        /// Ensures that MQ is ready for publisher configuration
        /// </summary>
        /// <param name="channel">MQ connection channel</param>
        /// <param name="configuration">Configuration</param>
        /// <returns></returns>
        public static void Publish(IModel channel, MessagePublishConfiguration configuration)
        {
            channel.ExchangeDeclare(configuration.ExchangeName, "topic", true);
        }

        /// <summary>
        /// Ensures that MQ is ready for consumer configuration
        /// </summary>
        /// <param name="channel">MQ connection channel</param>
        /// <param name="configuration">Configuration</param>
        /// <returns></returns>
        public static void Consume(IModel channel, MessageConsumeConfiguration configuration)
        {
            channel.QueueDeclare(configuration.QueueName, true, configuration.Exclusive, false,
                configuration.Arguments);
            channel.ExchangeDeclare(configuration.ExchangeName, "topic", true);

            foreach (var routingKey in configuration.RoutingKeys)
            {
                channel.QueueBind(configuration.QueueName, configuration.ExchangeName, routingKey);
            }
        }
    }
}