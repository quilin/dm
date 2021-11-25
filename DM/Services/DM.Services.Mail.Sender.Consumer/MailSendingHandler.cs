using System;
using System.Threading;
using System.Threading.Tasks;
using DM.Services.Core.Configuration;
using DM.Services.Core.Implementation.CorrelationToken;
using DM.Services.MessageQueuing;
using MailKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using Polly;
using Polly.Retry;

namespace DM.Services.Mail.Sender.Consumer
{
    /// <inheritdoc />
    internal class MailSendingHandler : IMessageHandler<MailLetter>
    {
        private readonly ILogger<MailSendingHandler> logger;
        private readonly ICorrelationTokenProvider correlationTokenProvider;
        private readonly EmailConfiguration configuration;
        private readonly Lazy<IMailTransport> client;
        private readonly AsyncRetryPolicy retryPolicy;

        /// <inheritdoc />
        public MailSendingHandler(
            IOptions<EmailConfiguration> configuration,
            ILogger<MailSendingHandler> logger,
            ICorrelationTokenProvider correlationTokenProvider)
        {
            this.logger = logger;
            this.correlationTokenProvider = correlationTokenProvider;
            this.configuration = configuration.Value;
            client = new Lazy<IMailTransport>(() =>
            {
                var smtpClient = new SmtpClient();
                smtpClient.Connect(this.configuration.ServerHost, this.configuration.ServerPort,
                    SecureSocketOptions.StartTls);
                smtpClient.Authenticate(this.configuration.Username, this.configuration.Password);
                smtpClient.NoOp();
                return smtpClient;
            });
            retryPolicy = Policy.Handle<Exception>().WaitAndRetryAsync(5,
                attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)),
                (exception, _, __) => logger.LogWarning(exception, "Something is wrong with mail sending"));
        }

        /// <inheritdoc />
        public async Task<ProcessResult> Handle(MailLetter message, CancellationToken cancellationToken)
        {
            logger.LogInformation($"Sending letter to {message.Address.Obfuscate()}");
            var policyResult = await retryPolicy.ExecuteAndCaptureAsync(() => client.Value.SendAsync(new MimeMessage
            {
                From = {new MailboxAddress(configuration.FromDisplayName, configuration.FromAddress)},
                ReplyTo = {new MailboxAddress(configuration.FromDisplayName, configuration.ReplyToAddress)},
                To = {MailboxAddress.Parse(message.Address)},
                Subject = message.Subject,
                Body = new TextPart(TextFormat.Html) {Text = message.Body},
                MessageId = correlationTokenProvider.Current.ToString()
            }, cancellationToken));
            return policyResult.Outcome == OutcomeType.Successful
                ? ProcessResult.Success
                : ProcessResult.Fail;
        }
    }
}