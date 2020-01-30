using System;
using System.Threading.Tasks;
using DM.Services.Community.BusinessProcesses.Registration.Confirmation;
using DM.Services.Core.Configuration;
using DM.Services.Mail.Rendering.Rendering;
using DM.Services.Mail.Sender;
using Microsoft.Extensions.Options;

namespace DM.Services.Community.BusinessProcesses.EmailChange.Confirmation
{
    /// <inheritdoc />
    public class EmailChangeMailSender : IEmailChangeMailSender
    {
        private readonly IRenderer renderer;
        private readonly IMailSender mailSender;
        private readonly IntegrationSettings integrationSettings;

        /// <inheritdoc />
        public EmailChangeMailSender(
            IRenderer renderer,
            IMailSender mailSender,
            IOptions<IntegrationSettings> integrationSettings)
        {
            this.renderer = renderer;
            this.mailSender = mailSender;
            this.integrationSettings = integrationSettings.Value;
        }

        /// <inheritdoc />
        public async Task Send(string email, string login, Guid token)
        {
            var confirmationLinkUri = new Uri(new Uri(integrationSettings.WebUrl), $"activate/{token}");
            var emailBody = await renderer.Render("RegistrationLetter", new RegistrationConfirmationViewModel
            {
                Login = login,
                ConfirmationLinkUrl = confirmationLinkUri.ToString()
            });
            await mailSender.Send(new MailLetter
            {
                Address = email,
                Subject = $"Подтверждение смены адреса электронной почты на DM.AM для {login}",
                Body = emailBody
            });
        }
    }
}