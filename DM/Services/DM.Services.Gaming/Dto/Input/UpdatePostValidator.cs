using DM.Services.Core.Exceptions;
using FluentValidation;

namespace DM.Services.Gaming.Dto.Input;

/// <inheritdoc />
internal class UpdatePostValidator : AbstractValidator<UpdatePost>
{
    /// <inheritdoc />
    public UpdatePostValidator()
    {
        RuleFor(p => p.PostId)
            .NotEmpty().WithMessage(ValidationError.Empty);
        When(p => p.Text != default, () =>
            RuleFor(p => p.Text)
                .NotEmpty().WithMessage(ValidationError.Empty));
    }
}