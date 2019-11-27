namespace DM.Services.MessageQueuing.Configuration
{
    /// <summary>
    /// Publisher configuration
    /// </summary>
    public class MessagePublishConfiguration
    {
        /// <summary>
        /// Exchange to publish to
        /// </summary>
        public string ExchangeName { get; set; }
    }
}