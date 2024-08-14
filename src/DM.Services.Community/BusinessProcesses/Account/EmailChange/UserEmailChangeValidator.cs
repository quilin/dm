using DM.Services.Authentication.Dto;
using DM.Services.Authentication.Implementation.Security;
using DM.Services.Core.Exceptions;
using FluentValidation;

namespace DM.Services.Community.BusinessProcesses.Account.EmailChange;

/// <inheritdoc />
internal class UserEmailChangeValidator : AbstractValidator<UserEmailChange>
{
    private const string FoundUserKey = nameof(FoundUserKey);

    /// <inheritdoc />
    public UserEmailChangeValidator(
        IEmailChangeRepository repository,
        ISecurityManager securityManager)
    {
        RuleFor(u => u.Login)
            .NotEmpty().WithMessage(ValidationError.Empty)
            .MustAsync(async (model, login, context, _) =>
            {
                var user = await repository.FindUser(login);
                if (user == null)
                {
                    return false;
                }

                context.RootContextData[FoundUserKey] = user;
                return true;
            }).WithMessage(ValidationError.Invalid);

        RuleFor(u => u.Password)
            .NotEmpty().WithMessage(ValidationError.Empty)
            .Must((model, password, context) =>
                context.RootContextData.TryGetValue(FoundUserKey, out var userWrapper) &&
                userWrapper is AuthenticatedUser user &&
                securityManager.ComparePasswords(password, user.Salt, user.PasswordHash))
            .WithMessage(ValidationError.Invalid);

        RuleFor(u => u.Email)
            .NotEmpty().WithMessage(ValidationError.Empty)
            .EmailAddress().WithMessage(ValidationError.Invalid);
    }
}