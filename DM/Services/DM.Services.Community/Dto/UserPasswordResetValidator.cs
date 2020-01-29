using System;
using DM.Services.Community.BusinessProcesses.Reading;
using DM.Services.Core.Exceptions;
using FluentValidation;

namespace DM.Services.Community.Dto
{
    /// <inheritdoc />
    public class UserPasswordResetValidator : AbstractValidator<UserPasswordReset>
    {
        private const string FoundEmailKey = nameof(FoundEmailKey);

        /// <inheritdoc />
        public UserPasswordResetValidator(
            IUserReadingRepository repository)
        {
            RuleFor(r => r.Login)
                .NotEmpty().WithMessage(ValidationError.Empty)
                .MustAsync(async (model, login, context, _) =>
                {
                    var user = await repository.GetUser(login);
                    if (user == null)
                    {
                        return false;
                    }

                    context.ParentContext.RootContextData[FoundEmailKey] = user.Email;
                    return true;
                }).WithMessage(ValidationError.Invalid);

            RuleFor(r => r.Email)
                .NotEmpty().WithMessage(ValidationError.Empty)
                .EmailAddress().WithMessage(ValidationError.Invalid)
                .Must((model, email, context) =>
                    context.ParentContext.RootContextData.TryGetValue(FoundEmailKey, out var userEmailWrapper) &&
                    userEmailWrapper is string userEmail &&
                    email.Equals(userEmail, StringComparison.InvariantCultureIgnoreCase))
                .WithMessage(ValidationError.Invalid);
        }
    }
}