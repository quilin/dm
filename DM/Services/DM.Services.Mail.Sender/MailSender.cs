using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using RMQ.Client.Abstractions;
using RMQ.Client.Abstractions.Producing;

namespace DM.Services.Mail.Sender
{
    /// <inheritdoc />
    internal class MailSender : IMailSender
    {
        private readonly IValidator<MailLetter> validator;
        private readonly IProducer producer;

        /// <inheritdoc />
        public MailSender(
            IValidator<MailLetter> validator,
            IProducerBuilder producerBuilder)
        {
            this.validator = validator;
            producer = producerBuilder.BuildRabbit(new RabbitProducerParameters("dm.mail.sending"));
        }

        /// <inheritdoc />
        public async Task Send(MailLetter letter)
        {
            await validator.ValidateAndThrowAsync(letter);
            await producer.Send(string.Empty, letter, CancellationToken.None);
        }
    }
}