using System;
using DM.Services.Authentication.Implementation.Security;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Core.Exceptions;
using DM.Services.Core.Implementation;
using FluentValidation;

namespace DM.Services.Community.BusinessProcesses.Account.PasswordChange;

/// <inheritdoc />
internal class UserPasswordChangeValidator : AbstractValidator<UserPasswordChange>
{
    /// <inheritdoc />
    public UserPasswordChangeValidator(
        IPasswordChangeRepository passwordChangeRepository,
        IDateTimeProvider dateTimeProvider,
        IIdentityProvider identityProvider,
        ISecurityManager securityManager)
    {
        When(c => c.Token.HasValue, () =>
                RuleFor(c => c.Token.Value)
                    .NotEmpty().WithMessage(ValidationError.Empty)
                    .MustAsync(async (token, _) =>
                    {
                        var tokenValid = await passwordChangeRepository.TokenValid(
                            token, dateTimeProvider.Now - TimeSpan.FromDays(1));
                        return tokenValid;
                    })
                    .WithMessage(ValidationError.Invalid))
            .Otherwise(() =>
            {
                RuleFor(c => c.OldPassword)
                    .NotEmpty().WithMessage(ValidationError.Empty)
                    .Must((model, password, context) =>
                        identityProvider.Current.User.IsAuthenticated &&
                        securityManager.ComparePasswords(password,
                            identityProvider.Current.User.Salt, identityProvider.Current.User.PasswordHash))
                    .WithMessage(ValidationError.Invalid);
            });

        RuleFor(r => r.NewPassword)
            .NotEmpty().WithMessage(ValidationError.Empty)
            .MinimumLength(6).WithMessage(ValidationError.Short);
    }
}