using FluentValidation;

namespace DM.Services.Authentication.Dto.Validations
{
    /// <summary>
    /// Validator for user registration DTO model
    /// </summary>
    public class UserRegistrationValidator : AbstractValidator<UserRegistration>
    {
        /// <inheritdoc />
        public UserRegistrationValidator()
        {
            RuleFor(r => r.Login)
                .NotEmpty()
                .MinimumLength(2)
                .MaximumLength(60);

            RuleFor(r => r.Email)
                .NotEmpty()
                .MaximumLength(100)
                .EmailAddress();

            RuleFor(r => r.Password)
                .NotEmpty()
                .MinimumLength(6);
        }
    }
}