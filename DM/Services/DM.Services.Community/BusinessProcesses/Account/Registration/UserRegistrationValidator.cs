using DM.Services.Core.Exceptions;
using FluentValidation;

namespace DM.Services.Community.BusinessProcesses.Account.Registration;

/// <summary>
/// Validator for user registration DTO model
/// </summary>
internal class UserRegistrationValidator : AbstractValidator<UserRegistration>
{
    /// <inheritdoc />
    public UserRegistrationValidator(
        IRegistrationRepository registrationRepository)
    {
        RuleFor(r => r.Login)
            .NotEmpty().WithMessage(ValidationError.Empty)
            .MinimumLength(2).WithMessage(ValidationError.Short)
            .MaximumLength(60).WithMessage(ValidationError.Long)
            .MustAsync(registrationRepository.LoginFree).WithMessage(ValidationError.Taken);

        RuleFor(r => r.Email)
            .NotEmpty().WithMessage(ValidationError.Empty)
            .MaximumLength(100).WithMessage(ValidationError.Long)
            .EmailAddress().WithMessage(ValidationError.Invalid)
            .MustAsync(registrationRepository.EmailFree).WithMessage(ValidationError.Taken);

        RuleFor(r => r.Password)
            .NotEmpty().WithMessage(ValidationError.Empty)
            .MinimumLength(6).WithMessage(ValidationError.Short);
    }
}