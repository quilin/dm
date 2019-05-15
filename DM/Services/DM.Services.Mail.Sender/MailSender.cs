using System.Threading.Tasks;
using DM.Services.MessageQueuing.Publish;
using FluentValidation;
using Microsoft.Extensions.Options;

namespace DM.Services.Mail.Sender
{
    /// <inheritdoc />
    public class MailSender : IMailSender
    {
        private readonly IValidator<MailLetter> validator;
        private readonly MailMessagePublishConfiguration publishConfiguration;
        private readonly IMessagePublisher publisher;

        /// <inheritdoc />
        public MailSender(
            IValidator<MailLetter> validator,
            IOptions<MailMessagePublishConfiguration> publishConfiguration,
            IMessagePublisher publisher)
        {
            this.validator = validator;
            this.publishConfiguration = publishConfiguration.Value;
            this.publisher = publisher;
        }
        
        /// <inheritdoc />
        public async Task Send(MailLetter letter)
        {
            await validator.ValidateAndThrowAsync(letter);
            await publisher.Publish(letter, publishConfiguration, "mail.sending");
        }
    }
}