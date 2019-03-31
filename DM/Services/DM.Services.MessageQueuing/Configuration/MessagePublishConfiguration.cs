namespace DM.Services.MessageQueuing.Configuration
{
    public class MessagePublishConfiguration
    {
        public string ExchangeName { get; set; }
        public string ExchangeType { get; set; }
        public string RoutingKey { get; set; }
        public string DurableQueueName { get; set; }
    }
}