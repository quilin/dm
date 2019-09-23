using DM.Services.MessageQueuing.Configuration;

namespace DM.Services.MessageQueuing.Publish
{
    /// <summary>
    /// Configuration for invoked dm event publisher
    /// </summary>
    public class DmEventPublishConfiguration : IMessagePublishConfiguration
    {
        /// <inheritdoc />
        public string ExchangeName { get; set; }
    }
}