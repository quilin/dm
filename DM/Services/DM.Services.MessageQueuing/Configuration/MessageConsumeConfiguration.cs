using System.Collections.Generic;

namespace DM.Services.MessageQueuing.Configuration
{
    public class MessageConsumeConfiguration
    {
        public string ExchangeName { get; set; }
        public IEnumerable<string> RoutingKeys { get; set; }
        public string QueueName { get; set; }
    }
}