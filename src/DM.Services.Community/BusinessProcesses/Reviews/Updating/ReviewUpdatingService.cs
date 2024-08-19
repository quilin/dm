using System.Threading;
using System.Threading.Tasks;
using DM.Services.Common.Authorization;
using DM.Services.Community.BusinessProcesses.Reviews.Reading;
using DM.Services.DataAccess.RelationalStorage;
using FluentValidation;
using DbReview = DM.Services.DataAccess.BusinessObjects.Common.Review;

namespace DM.Services.Community.BusinessProcesses.Reviews.Updating;

/// <inheritdoc />
internal class ReviewUpdatingService(
    IValidator<UpdateReview> validator,
    IReviewReadingService readingService,
    IIntentionManager intentionManager,
    IUpdateBuilderFactory updateBuilderFactory,
    IReviewUpdatingRepository repository) : IReviewUpdatingService
{
    /// <inheritdoc />
    public async Task<Review> Update(UpdateReview updateReview, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(updateReview, cancellationToken);
        var review = await readingService.Get(updateReview.ReviewId, cancellationToken);

        var reviewChanges = updateBuilderFactory.Create<DbReview>(review.Id);
        if (updateReview.Approved.HasValue && updateReview.Approved.Value != review.Approved)
        {
            intentionManager.ThrowIfForbidden(ReviewIntention.Approve, review);
            reviewChanges.MaybeField(r => r.IsApproved, updateReview.Approved);
        }

        if (!string.IsNullOrEmpty(updateReview.Text))
        {
            intentionManager.ThrowIfForbidden(ReviewIntention.Edit, review);
            reviewChanges.MaybeField(r => r.Text, updateReview.Text.Trim());
        }

        var result = await repository.Update(reviewChanges, cancellationToken);
        return result;
    }
}