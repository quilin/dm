using DM.Services.MessageQueuing.Configuration;

namespace DM.Services.Mail.Sender
{
    /// <summary>
    /// Configuration for mail MQ sender
    /// </summary>
    public class MailMessagePublishConfiguration : IMessagePublishConfiguration
    {
        /// <inheritdoc />
        public string ExchangeName { get; set; }
    }
}