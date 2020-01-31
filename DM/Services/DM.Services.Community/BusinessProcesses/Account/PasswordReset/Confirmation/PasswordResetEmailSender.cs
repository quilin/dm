using System;
using System.Threading.Tasks;
using DM.Services.Core.Configuration;
using DM.Services.Mail.Rendering.Rendering;
using DM.Services.Mail.Sender;
using Microsoft.Extensions.Options;

namespace DM.Services.Community.BusinessProcesses.Account.PasswordReset.Confirmation
{
    /// <inheritdoc />
    public class PasswordResetEmailSender : IPasswordResetEmailSender
    {
        private readonly IRenderer renderer;
        private readonly IMailSender mailSender;
        private readonly IntegrationSettings integrationSettings;

        /// <inheritdoc />
        public PasswordResetEmailSender(
            IRenderer renderer,
            IMailSender mailSender,
            IOptions<IntegrationSettings> integrationOptions)
        {
            this.renderer = renderer;
            this.mailSender = mailSender;
            integrationSettings = integrationOptions.Value;
        }

        /// <inheritdoc />
        public async Task Send(string email, string login, Guid token)
        {
            var confirmationLinkUri = new Uri(new Uri(integrationSettings.WebUrl), $"password/{token}");
            var emailBody = await renderer.Render("PasswordResetLetter", new PasswordResetConfirmationViewModel
            {
                Login = login,
                ConfirmationLinkUri = confirmationLinkUri.ToString()
            });
            await mailSender.Send(new MailLetter
            {
                Address = email,
                Subject = $"Подтверждение сброса пароля на DM.AM для {login}",
                Body = emailBody
            });
        }
    }
}