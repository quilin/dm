using DM.Services.Core.Exceptions;
using FluentValidation;

namespace DM.Services.Forum.Dto.Input;

/// <summary>
/// Validator for topic modification DTO model
/// </summary>
internal class UpdateTopicValidator : AbstractValidator<UpdateTopic>
{
    /// <inheritdoc />
    public UpdateTopicValidator()
    {
        RuleFor(t => t.TopicId)
            .NotEmpty().WithMessage(ValidationError.Empty);

        When(t => t.Title != null, () =>
            RuleFor(t => t.Title)
                .NotEmpty().WithMessage(ValidationError.Empty)
                .MaximumLength(130).WithMessage(ValidationError.Long));
    }
}