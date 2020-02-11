using DM.Services.Core.Exceptions;
using FluentValidation;

namespace DM.Services.Community.BusinessProcesses.Reviews.Creating
{
    /// <inheritdoc />
    public class CreateReviewValidator : AbstractValidator<CreateReview>
    {
        /// <inheritdoc />
        public CreateReviewValidator()
        {
            RuleFor(r => r.Text)
                .NotEmpty().WithMessage(ValidationError.Empty);
        }
    }
}