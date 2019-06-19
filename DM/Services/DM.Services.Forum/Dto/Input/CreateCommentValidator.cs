using DM.Services.Core.Exceptions;
using FluentValidation;

namespace DM.Services.Forum.Dto.Input
{
    /// <summary>
    /// Validator for commentary creation DTO model
    /// </summary>
    public class CreateCommentValidator : AbstractValidator<CreateComment>
    {
        /// <inheritdoc />
        public CreateCommentValidator()
        {
            RuleFor(c => c.Text)
                .NotEmpty().WithMessage(ValidationError.Empty);
            RuleFor(c => c.TopicId)
                .NotEmpty();
        }
    }
}