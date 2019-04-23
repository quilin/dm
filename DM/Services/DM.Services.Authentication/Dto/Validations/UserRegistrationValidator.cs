using DM.Services.Authentication.Repositories;
using FluentValidation;

namespace DM.Services.Authentication.Dto.Validations
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
                .NotEmpty()
                .MinimumLength(2)
                .MaximumLength(60)
                .MustAsync(registrationRepository.LoginFree);

            RuleFor(r => r.Email)
                .NotEmpty()
                .MaximumLength(100)
                .EmailAddress()
                .MustAsync(registrationRepository.EmailFree);

            RuleFor(r => r.Password)
                .NotEmpty()
                .MinimumLength(6);
        }
    }
}