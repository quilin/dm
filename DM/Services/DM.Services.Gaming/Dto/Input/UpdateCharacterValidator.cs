using DM.Services.Core.Exceptions;
using FluentValidation;

namespace DM.Services.Gaming.Dto.Input
{
    /// <inheritdoc />
    public class UpdateCharacterValidator : AbstractValidator<UpdateCharacter>
    {
        /// <inheritdoc />
        public UpdateCharacterValidator()
        {
            When(c => c.Name != default, () =>
                RuleFor(c => c.Name)
                    .NotEmpty().WithMessage(ValidationError.Empty)
                    .MaximumLength(50).WithMessage(ValidationError.Long));

            When(c => c.Race != default, () =>
                RuleFor(c => c.Race)
                    .MaximumLength(30).WithMessage(ValidationError.Long));

            When(c => c.Race != default, () =>
                RuleFor(c => c.Class)
                    .MaximumLength(30).WithMessage(ValidationError.Long));
        }
    }
}