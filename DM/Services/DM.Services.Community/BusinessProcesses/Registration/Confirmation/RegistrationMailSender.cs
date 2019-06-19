using System;
using System.Threading.Tasks;
using DM.Services.Core.Configuration;
using DM.Services.Core.Rendering;
using DM.Services.Mail.Sender;
using Microsoft.Extensions.Options;

namespace DM.Services.Community.BusinessProcesses.Registration.Confirmation
{
    /// <inheritdoc />
    public class RegistrationMailSender : IRegistrationMailSender
    {
        private readonly ITemplateRenderer renderer;
        private readonly IMailSender mailSender;
        private readonly IntegrationSettings integrationSettings;

        /// <inheritdoc />
        public RegistrationMailSender(
            ITemplateRenderer renderer,
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
            await mailSender.Send(new MailLetter
            {
                Address = email,
                Subject = $"Добро пожаловать на DM.AM, {login}!",
                Body = await renderer.Render("RegistrationLetter", new RegistrationConfirmationViewModel
                {
                    Login = login,
                    ConfirmationLinkUrl = confirmationLinkUri.ToString()
                })
            });
        }
    }
}