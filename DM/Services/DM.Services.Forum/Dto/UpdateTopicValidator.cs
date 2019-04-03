using FluentValidation;

namespace DM.Services.Forum.Dto
{
    /// <summary>
    /// Validator for topic modification DTO model
    /// </summary>
    public class UpdateTopicValidator : AbstractValidator<UpdateTopic>
    {
        /// <inheritdoc />
        public UpdateTopicValidator()
        {
            RuleFor(t => t.Title)
                .NotEmpty()
                .MaximumLength(130);
        }
    }
}