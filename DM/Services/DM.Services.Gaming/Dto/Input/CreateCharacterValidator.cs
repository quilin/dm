using DM.Services.Core.Exceptions;
using FluentValidation;

namespace DM.Services.Gaming.Dto.Input
{
    /// <summary>
    /// Validator for character creation DTO
    /// </summary>
    public class CreateCharacterValidator : AbstractValidator<CreateCharacter>
    {
        /// <inheritdoc />
        public CreateCharacterValidator()
        {
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage(ValidationError.Empty)
                .MaximumLength(50).WithMessage(ValidationError.Long);

            RuleFor(c => c.Race)
                .MaximumLength(30).WithMessage(ValidationError.Long);

            RuleFor(c => c.Class)
                .MaximumLength(30).WithMessage(ValidationError.Long);
        }
    }
}