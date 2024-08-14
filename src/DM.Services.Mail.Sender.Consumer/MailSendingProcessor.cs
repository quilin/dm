using System;
using System.Threading;
using System.Threading.Tasks;
using DM.Services.Core.Configuration;
using DM.Services.Core.Implementation.CorrelationToken;
using Jamq.Client.Abstractions.Consuming;
using MailKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

namespace DM.Services.Mail.Sender.Consumer;

/// <inheritdoc />
internal class MailSendingProcessor : IProcessor<string, MailLetter>
{
    private readonly ILogger<MailSendingProcessor> logger;
    private readonly ICorrelationTokenProvider correlationTokenProvider;
    private readonly EmailConfiguration configuration;
    private readonly Lazy<IMailTransport> client;

    /// <inheritdoc />
    public MailSendingProcessor(
        IOptions<EmailConfiguration> configuration,
        ILogger<MailSendingProcessor> logger,
        ICorrelationTokenProvider correlationTokenProvider)
    {
        this.logger = logger;
        this.correlationTokenProvider = correlationTokenProvider;
        this.configuration = configuration.Value;
        client = new Lazy<IMailTransport>(() =>
        {
            var smtpClient = new SmtpClient();
            smtpClient.Connect(this.configuration.ServerHost, this.configuration.ServerPort,
                SecureSocketOptions.StartTlsWhenAvailable);
            smtpClient.Authenticate(this.configuration.Username, this.configuration.Password);
            smtpClient.NoOp();
            return smtpClient;
        });
    }

    /// <inheritdoc />
    public async Task<ProcessResult> Process(string key, MailLetter message, CancellationToken cancellationToken)
    {
        logger.LogInformation("Sending letter to {Address}", message.Address.Obfuscate());
        await client.Value.SendAsync(new MimeMessage
        {
            From = {new MailboxAddress(configuration.FromDisplayName, configuration.FromAddress)},
            ReplyTo = {new MailboxAddress(configuration.FromDisplayName, configuration.ReplyToAddress)},
            To = {MailboxAddress.Parse(message.Address)},
            Subject = message.Subject,
            Body = new TextPart(TextFormat.Html) {Text = message.Body},
            MessageId = correlationTokenProvider.Current.ToString()
        }, cancellationToken);
        return ProcessResult.Success;
    }
}