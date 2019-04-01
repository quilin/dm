using System.Collections.Generic;

namespace DM.Services.MessageQueuing.Configuration
{
    /// <summary>
    /// Consumer configuration
    /// </summary>
    public class MessageConsumeConfiguration
    {
        /// <summary>
        /// Exchange to bind queue to
        /// </summary>
        public string ExchangeName { get; set; }

        /// <summary>
        /// Routing keys to bind queue by
        /// </summary>
        public IEnumerable<string> RoutingKeys { get; set; }

        /// <summary>
        /// Consumer queue
        /// </summary>
        public string QueueName { get; set; }
    }
}