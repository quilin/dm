using System.Collections.Generic;

namespace DM.Services.MessageQueuing.Configuration
{
    /// <summary>
    /// Consumer configuration
    /// </summary>
    public interface IMessageConsumeConfiguration
    {
        /// <summary>
        /// Consumer queue
        /// </summary>
        string QueueName { get; set; }

        /// <summary>
        /// Source exchange
        /// </summary>
        string ExchangeName { get; set; }

        /// <summary>
        /// Binding routes for queue
        /// </summary>
        IEnumerable<string> RoutingKeys { get; set; }

        /// <summary>
        /// Queue arguments
        /// </summary>
        IDictionary<string, object> Arguments { get; set; }

        /// <summary>
        /// Queue exclusiveness flag
        /// </summary>
        bool Exclusive { get; set; }
    }
}