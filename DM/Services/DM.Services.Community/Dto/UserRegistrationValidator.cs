using DM.Services.Community.BusinessProcesses.Registration;
using FluentValidation;

namespace DM.Services.Community.Dto
{
    /// <summary>
    /// Validator for user registration DTO model
    /// </summary>
    public class UserRegistrationValidator : AbstractValidator<UserRegistration>
    {
        /// <inheritdoc />
        public UserRegistrationValidator(
            IRegistrationRepository registrationRepository)
        {
            RuleFor(r => r.Login)
                .NotEmpty().WithMessage("Login must not be empty")
                .MinimumLength(2).WithMessage("Login must be at least 2 characters long")
                .MaximumLength(60).WithMessage("Login must be at most 60 characters long")
                .MustAsync(registrationRepository.LoginFree).WithMessage("Login must be unique");

            RuleFor(r => r.Email)
                .NotEmpty().WithMessage("Email must not be empty")
                .MaximumLength(100).WithMessage("Email must be at most 100 characters long")
                .EmailAddress().WithMessage("Email must be a valid address")
                .MustAsync(registrationRepository.EmailFree).WithMessage("Email must be unique");

            RuleFor(r => r.Password)
                .NotEmpty().WithMessage("Password must not be empty")
                .MinimumLength(6).WithMessage("Password must be at least 6 character long. It's for your own safety!");
        }
    }
}