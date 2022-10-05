using DM.Services.Core.Exceptions;
using FluentValidation;

namespace DM.Services.Gaming.Dto.Input;

/// <inheritdoc />
internal class CreatePostValidator : AbstractValidator<CreatePost>
{
    /// <inheritdoc />
    public CreatePostValidator()
    {
        RuleFor(p => p.RoomId)
            .NotEmpty().WithMessage(ValidationError.Empty);
        RuleFor(p => p.Text)
            .NotEmpty().WithMessage(ValidationError.Empty);
    }
}