using System;
using DM.Services.Authentication.Dto;
using DM.Services.Authentication.Implementation.Security;
using DM.Services.Community.BusinessProcesses.PasswordChange;
using DM.Services.Core.Dto;
using DM.Services.Core.Exceptions;
using DM.Services.Core.Implementation;
using FluentValidation;

namespace DM.Services.Community.Dto
{
    /// <inheritdoc />
    public class UserPasswordChangeValidator : AbstractValidator<UserPasswordChange>
    {
        private const string FoundUserKey = nameof(FoundUserKey);

        /// <inheritdoc />
        public UserPasswordChangeValidator(
            IPasswordChangeRepository passwordChangeRepository,
            IDateTimeProvider dateTimeProvider,
            ISecurityManager securityManager)
        {
            RuleFor(c => c.Login)
                .NotEmpty().WithMessage(ValidationError.Empty)
                .MustAsync(async (model, login, context, _) =>
                {
                    var user = await passwordChangeRepository.FindUser(login);
                    if (user == null)
                    {
                        return false;
                    }

                    context.ParentContext.RootContextData[FoundUserKey] = user;
                    return true;
                }).WithMessage(ValidationError.Invalid);

            When(c => c.Token.HasValue, () =>
                    RuleFor(c => c.Token.Value)
                        .NotEmpty().WithMessage(ValidationError.Empty)
                        .MustAsync(async (model, token, context, _) =>
                            context.ParentContext.RootContextData.TryGetValue(FoundUserKey, out var userWrapper) &&
                            userWrapper is GeneralUser user &&
                            await passwordChangeRepository.TokenValid(
                                token, user.UserId, dateTimeProvider.Now - TimeSpan.FromDays(1)))
                        .WithMessage(ValidationError.Invalid))
                .Otherwise(() =>
                    RuleFor(c => c.OldPassword)
                        .NotEmpty().WithMessage(ValidationError.Empty)
                        .Must((model, password, context) =>
                            context.ParentContext.RootContextData.TryGetValue(FoundUserKey, out var userWrapper) &&
                            userWrapper is AuthenticatedUser user &&
                            securityManager.ComparePasswords(password, user.Salt, user.PasswordHash))
                        .WithMessage(ValidationError.Invalid));

            RuleFor(r => r.NewPassword)
                .NotEmpty().WithMessage(ValidationError.Empty)
                .MinimumLength(6).WithMessage(ValidationError.Short);
        }
    }
}