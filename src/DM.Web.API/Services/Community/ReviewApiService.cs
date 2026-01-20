using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DM.Services.Community.BusinessProcesses.Reviews.Creating;
using DM.Services.Community.BusinessProcesses.Reviews.Deleting;
using DM.Services.Community.BusinessProcesses.Reviews.Reading;
using DM.Services.Community.BusinessProcesses.Reviews.Updating;
using DM.Web.API.Dto.Community;
using DM.Web.API.Dto.Contracts;
using Review = DM.Web.API.Dto.Community.Review;

namespace DM.Web.API.Services.Community;

/// <inheritdoc />
internal class ReviewApiService(
    IReviewReadingService readingService,
    IReviewCreatingService creatingService,
    IReviewUpdatingService updatingService,
    IReviewDeletingService deletingService,
    IMapper mapper) : IReviewApiService
{
    /// <inheritdoc />
    public async Task<ListEnvelope<Review>> Get(ReviewsQuery query, CancellationToken cancellationToken)
    {
        var (reviews, paging) = await readingService.Get(query, query.OnlyApproved, cancellationToken);
        return new ListEnvelope<Review>(reviews.Select(mapper.Map<Review>), new Paging(paging));
    }

    /// <inheritdoc />
    public async Task<Envelope<Review>> Get(Guid id, CancellationToken cancellationToken)
    {
        var review = await readingService.Get(id, cancellationToken);
        return new Envelope<Review>(mapper.Map<Review>(review));
    }

    /// <inheritdoc />
    public async Task<Envelope<Review>> Create(Review review, CancellationToken cancellationToken)
    {
        var createReview = mapper.Map<CreateReview>(review);
        var createdReview = await creatingService.Create(createReview, cancellationToken);
        return new Envelope<Review>(mapper.Map<Review>(createdReview));
    }

    /// <inheritdoc />
    public async Task<Envelope<Review>> Update(Guid id, Review review, CancellationToken cancellationToken)
    {
        var updateReview = mapper.Map<UpdateReview>(review);
        updateReview.ReviewId = id;
        var updatedReview = await updatingService.Update(updateReview, cancellationToken);
        return new Envelope<Review>(mapper.Map<Review>(updatedReview));
    }

    /// <inheritdoc />
    public Task Delete(Guid id, CancellationToken cancellationToken) =>
        deletingService.Delete(id, cancellationToken);
}