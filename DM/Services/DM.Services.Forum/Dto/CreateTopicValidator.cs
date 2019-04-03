using FluentValidation;

namespace DM.Services.Forum.Dto
{
    /// <summary>
    /// Validator for topic creation DTO model
    /// </summary>
    public class CreateTopicValidator : AbstractValidator<CreateTopic>
    {
        /// <inheritdoc />
        public CreateTopicValidator()
        {
            RuleFor(t => t.Title)
                .NotEmpty()
                .MaximumLength(130);
        }
    }
}