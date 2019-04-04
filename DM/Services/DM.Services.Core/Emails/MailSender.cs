using System;
using System.Threading.Tasks;
using DM.Services.Core.Configuration;
using MailKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

namespace DM.Services.Core.Emails
{
    /// <inheritdoc />
    public class MailSender : IMailSender
    {
        private readonly Lazy<IMailTransport> client;
        private readonly EmailConfiguration configuration;

        /// <inheritdoc />
        public MailSender(
            IOptions<EmailConfiguration> emailOptions)
        {
            configuration = emailOptions.Value;
            client = new Lazy<IMailTransport>(() =>
            {
                var smtpClient = new SmtpClient();
                smtpClient.Connect(configuration.ServerHost, configuration.ServerPort,
                    SecureSocketOptions.StartTls);
                smtpClient.Authenticate(configuration.Username, configuration.Password);
                return smtpClient;
            });
        }

        /// <inheritdoc />
        public Task Send(string address, string subject, string body)
        {
            return client.Value.SendAsync(new MimeMessage
            {
                From = {new MailboxAddress(configuration.FromAddress)},
                ReplyTo = {new MailboxAddress(configuration.ReplyToAddress)},
                To = {new MailboxAddress(address)},
                Subject = subject,
                Body = new TextPart(TextFormat.Html) {Text = body}
            });
        }
    }
}