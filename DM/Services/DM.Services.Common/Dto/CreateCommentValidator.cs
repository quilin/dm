using DM.Services.Core.Exceptions;
using FluentValidation;

namespace DM.Services.Common.Dto;

/// <summary>
/// Validator for commentary creation DTO model
/// </summary>
internal class CreateCommentValidator : AbstractValidator<CreateComment>
{
    /// <inheritdoc />
    public CreateCommentValidator()
    {
        RuleFor(c => c.Text)
            .NotEmpty().WithMessage(ValidationError.Empty);
        RuleFor(c => c.EntityId)
            .NotEmpty();
    }
}