using System;
using DM.Services.Community.BusinessProcesses.Users.Reading;
using DM.Services.Core.Exceptions;
using FluentValidation;

namespace DM.Services.Community.BusinessProcesses.Account.PasswordReset;

/// <inheritdoc />
internal class UserPasswordResetValidator : AbstractValidator<UserPasswordReset>
{
    private const string FoundEmailKey = nameof(FoundEmailKey);

    /// <inheritdoc />
    public UserPasswordResetValidator(
        IUserReadingRepository repository)
    {
        RuleFor(r => r.Login)
            .NotEmpty().WithMessage(ValidationError.Empty)
            .MustAsync(async (_, login, context, _) =>
            {
                var user = await repository.GetUserDetails(login);
                if (user == null)
                {
                    return false;
                }

                context.RootContextData[FoundEmailKey] = user.Email;
                return true;
            }).WithMessage(ValidationError.Invalid);

        RuleFor(r => r.Email)
            .NotEmpty().WithMessage(ValidationError.Empty)
            .EmailAddress().WithMessage(ValidationError.Invalid)
            .Must((_, email, context) =>
                context.RootContextData.TryGetValue(FoundEmailKey, out var userEmailWrapper) &&
                userEmailWrapper is string userEmail &&
                email.Equals(userEmail, StringComparison.InvariantCultureIgnoreCase))
            .WithMessage(ValidationError.Invalid);
    }
}