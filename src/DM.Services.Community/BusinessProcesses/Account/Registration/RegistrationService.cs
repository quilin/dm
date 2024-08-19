using System.Threading;
using System.Threading.Tasks;
using DM.Services.Authentication.Implementation.Security;
using DM.Services.Community.BusinessProcesses.Account.Activation;
using DM.Services.Community.BusinessProcesses.Account.Registration.Confirmation;
using DM.Services.Core.Dto.Enums;
using DM.Services.MessageQueuing.GeneralBus;
using FluentValidation;

namespace DM.Services.Community.BusinessProcesses.Account.Registration;

/// <inheritdoc />
internal class RegistrationService(
    IValidator<UserRegistration> validator,
    ISecurityManager securityManager,
    IUserFactory userFactory,
    IActivationTokenFactory activationTokenFactory,
    IRegistrationRepository repository,
    IRegistrationMailSender mailSender,
    IInvokedEventProducer producer) : IRegistrationService
{
    /// <inheritdoc />
    public async Task Register(UserRegistration registration, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(registration, cancellationToken);

        var (hash, salt) = securityManager.GeneratePassword(registration.Password);
        var user = userFactory.Create(registration, salt, hash);
        var token = activationTokenFactory.Create(user.UserId);

        await repository.AddUser(user, token, cancellationToken);
        await mailSender.Send(user.Email, user.Login, token.TokenId, cancellationToken);
        await producer.Send(EventType.NewUser, user.UserId);
    }
}