using FluentValidation;

namespace DM.Services.Forum.Dto.Input
{
    /// <summary>
    /// Validator for commentary updating DTO model
    /// </summary>
    public class UpdateCommentValidator : AbstractValidator<UpdateComment>
    {
        /// <inheritdoc />
        public UpdateCommentValidator()
        {
            RuleFor(c => c.CommentId)
                .NotEmpty();
            RuleFor(c => c.Text)
                .NotEmpty();
        }
    }
}