namespace DM.Services.MessageQueuing.Configuration
{
    /// <summary>
    /// Publisher configuration
    /// </summary>
    public interface IMessagePublishConfiguration
    {
        /// <summary>
        /// Exchange to publish to
        /// </summary>
        string ExchangeName { get; set; }
    }
}