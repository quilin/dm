using System.Threading.Tasks;
using DM.Services.MessageQueuing.Configuration;
using DM.Services.MessageQueuing.Publish;
using FluentValidation;

namespace DM.Services.Mail.Sender
{
    /// <inheritdoc />
    public class MailSender : IMailSender
    {
        private readonly IValidator<MailLetter> validator;
        private readonly IMessagePublisher publisher;

        /// <inheritdoc />
        public MailSender(
            IValidator<MailLetter> validator,
            IMessagePublisher publisher)
        {
            this.validator = validator;
            this.publisher = publisher;
        }

        /// <inheritdoc />
        public async Task Send(MailLetter letter)
        {
            await validator.ValidateAndThrowAsync(letter);
            await publisher.Publish(letter, new MessagePublishConfiguration
            {
                ExchangeName = "dm.mail.sending"
            }, string.Empty);
        }
    }
}