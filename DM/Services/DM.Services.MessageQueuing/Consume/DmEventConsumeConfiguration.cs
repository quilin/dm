using System.Collections.Generic;
using DM.Services.MessageQueuing.Configuration;

namespace DM.Services.MessageQueuing.Consume
{
    /// <summary>
    /// Consumer configuration of dm event messages
    /// </summary>
    public class DmEventConsumeConfiguration : IMessageConsumeConfiguration
    {
        /// <inheritdoc />
        public string QueueName { get; set; }

        /// <inheritdoc />
        public string ExchangeName { get; set; }

        /// <inheritdoc />
        public IEnumerable<string> RoutingKeys { get; set; }

        /// <inheritdoc />
        public IDictionary<string, object> Arguments { get; set; }

        /// <inheritdoc />
        public bool Exclusive { get; set; }
    }
}