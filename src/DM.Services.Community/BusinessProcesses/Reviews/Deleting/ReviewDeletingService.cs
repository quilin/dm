using System;
using System.Threading.Tasks;
using DM.Services.Common.Authorization;
using DM.Services.Community.BusinessProcesses.Reviews.Reading;
using DM.Services.Community.BusinessProcesses.Reviews.Updating;
using DM.Services.DataAccess.RelationalStorage;
using Review = DM.Services.DataAccess.BusinessObjects.Common.Review;

namespace DM.Services.Community.BusinessProcesses.Reviews.Deleting;

/// <inheritdoc />
internal class ReviewDeletingService : IReviewDeletingService
{
    private readonly IReviewReadingService reviewReadingService;
    private readonly IIntentionManager intentionManager;
    private readonly IUpdateBuilderFactory updateBuilderFactory;
    private readonly IReviewUpdatingRepository repository;

    /// <inheritdoc />
    public ReviewDeletingService(
        IReviewReadingService reviewReadingService,
        IIntentionManager intentionManager,
        IUpdateBuilderFactory updateBuilderFactory,
        IReviewUpdatingRepository repository)
    {
        this.reviewReadingService = reviewReadingService;
        this.intentionManager = intentionManager;
        this.updateBuilderFactory = updateBuilderFactory;
        this.repository = repository;
    }

    /// <inheritdoc />
    public async Task Delete(Guid id)
    {
        var review = await reviewReadingService.Get(id);
        intentionManager.ThrowIfForbidden(ReviewIntention.Delete, review);
        var deleteReview = updateBuilderFactory.Create<Review>(id).Field(r => r.IsRemoved, true);
        await repository.Update(deleteReview);
    }
}