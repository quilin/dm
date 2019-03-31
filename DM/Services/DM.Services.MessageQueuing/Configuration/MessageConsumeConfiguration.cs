namespace DM.Services.MessageQueuing.Configuration
{
    public class MessageConsumeConfiguration
    {
        public string ExchangeName { get; set; }
        public string RoutingKey { get; set; }
        public string QueueName { get; set; }
    }
}