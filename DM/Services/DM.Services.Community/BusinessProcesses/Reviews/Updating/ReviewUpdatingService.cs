using System.Threading.Tasks;
using DM.Services.Common.Authorization;
using DM.Services.Community.BusinessProcesses.Reviews.Reading;
using DM.Services.DataAccess.RelationalStorage;
using FluentValidation;
using DbReview = DM.Services.DataAccess.BusinessObjects.Common.Review;

namespace DM.Services.Community.BusinessProcesses.Reviews.Updating;

/// <inheritdoc />
internal class ReviewUpdatingService : IReviewUpdatingService
{
    private readonly IValidator<UpdateReview> validator;
    private readonly IReviewReadingService readingService;
    private readonly IIntentionManager intentionManager;
    private readonly IUpdateBuilderFactory updateBuilderFactory;
    private readonly IReviewUpdatingRepository repository;

    /// <inheritdoc />
    public ReviewUpdatingService(
        IValidator<UpdateReview> validator,
        IReviewReadingService readingService,
        IIntentionManager intentionManager,
        IUpdateBuilderFactory updateBuilderFactory,
        IReviewUpdatingRepository repository)
    {
        this.validator = validator;
        this.readingService = readingService;
        this.intentionManager = intentionManager;
        this.updateBuilderFactory = updateBuilderFactory;
        this.repository = repository;
    }
        
    /// <inheritdoc />
    public async Task<Review> Update(UpdateReview updateReview)
    {
        await validator.ValidateAndThrowAsync(updateReview);
        var review = await readingService.Get(updateReview.ReviewId);

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

        var result = await repository.Update(reviewChanges);
        return result;
    }
}