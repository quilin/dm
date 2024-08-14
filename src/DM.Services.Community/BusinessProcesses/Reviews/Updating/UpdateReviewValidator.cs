using DM.Services.Core.Exceptions;
using FluentValidation;

namespace DM.Services.Community.BusinessProcesses.Reviews.Updating;

/// <inheritdoc />
internal class UpdateReviewValidator : AbstractValidator<UpdateReview>
{
    /// <inheritdoc />
    public UpdateReviewValidator()
    {
        RuleFor(r => r.ReviewId)
            .NotEmpty().WithMessage(ValidationError.Empty);

        When(r => r.Text != null, () =>
            RuleFor(r => r.Text)
                .NotEmpty().WithMessage(ValidationError.Empty));
    }
}