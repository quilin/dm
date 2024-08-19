using System;
using System.Threading;
using System.Threading.Tasks;
using DM.Services.Core.Configuration;
using DM.Services.Mail.Rendering.Rendering;
using DM.Services.Mail.Sender;
using Microsoft.Extensions.Options;

namespace DM.Services.Community.BusinessProcesses.Account.Registration.Confirmation;

/// <inheritdoc />
internal class RegistrationMailSender : IRegistrationMailSender
{
    private readonly IRenderer renderer;
    private readonly IMailSender mailSender;
    private readonly IntegrationSettings integrationSettings;

    /// <inheritdoc />
    public RegistrationMailSender(
        IRenderer renderer,
        IMailSender mailSender,
        IOptions<IntegrationSettings> integrationSettings)
    {
        this.renderer = renderer;
        this.mailSender = mailSender;
        this.integrationSettings = integrationSettings.Value;
    }

    /// <inheritdoc />
    public async Task Send(string email, string login, Guid token, CancellationToken cancellationToken)
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
            Subject = $"Добро пожаловать на DM.AM, {login}!",
            Body = emailBody
        });
    }
}