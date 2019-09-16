using System.Collections.Generic;
using DM.Services.MessageQueuing.Configuration;

namespace DM.Services.Mail.Sender.Consumer
{
    /// <summary>
    /// Configuration of mail sender consumer
    /// </summary>
    public class MailSenderConsumeConfiguration : IMessageConsumeConfiguration
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