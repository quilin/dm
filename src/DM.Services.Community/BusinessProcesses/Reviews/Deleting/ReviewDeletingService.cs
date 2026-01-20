using System;
using System.Threading;
using System.Threading.Tasks;
using DM.Services.Common.Authorization;
using DM.Services.Community.BusinessProcesses.Reviews.Reading;
using DM.Services.Community.BusinessProcesses.Reviews.Updating;
using DM.Services.DataAccess.RelationalStorage;
using Review = DM.Services.DataAccess.BusinessObjects.Common.Review;

namespace DM.Services.Community.BusinessProcesses.Reviews.Deleting;

/// <inheritdoc />
internal class ReviewDeletingService(
    IReviewReadingService reviewReadingService,
    IIntentionManager intentionManager,
    IUpdateBuilderFactory updateBuilderFactory,
    IReviewUpdatingRepository repository) : IReviewDeletingService
{
    /// <inheritdoc />
    public async Task Delete(Guid id, CancellationToken cancellationToken)
    {
        var review = await reviewReadingService.Get(id, cancellationToken);
        intentionManager.ThrowIfForbidden(ReviewIntention.Delete, review);
        var deleteReview = updateBuilderFactory.Create<Review>(id).Field(r => r.IsRemoved, true);
        await repository.Update(deleteReview, cancellationToken);
    }
}