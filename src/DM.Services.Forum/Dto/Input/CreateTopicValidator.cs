using DM.Services.Core.Exceptions;
using FluentValidation;

namespace DM.Services.Forum.Dto.Input;

/// <summary>
/// Validator for topic creation DTO model
/// </summary>
internal class CreateTopicValidator : AbstractValidator<CreateTopic>
{
    /// <inheritdoc />
    public CreateTopicValidator()
    {
        RuleFor(t => t.Title)
            .NotEmpty().WithMessage(ValidationError.Empty)
            .MaximumLength(130).WithMessage(ValidationError.Long);
    }
}