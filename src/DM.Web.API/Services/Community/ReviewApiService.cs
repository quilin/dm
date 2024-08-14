using System;
using System.Linq;
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
internal class ReviewApiService : IReviewApiService
{
    private readonly IReviewReadingService readingService;
    private readonly IReviewCreatingService creatingService;
    private readonly IReviewUpdatingService updatingService;
    private readonly IReviewDeletingService deletingService;
    private readonly IMapper mapper;

    /// <inheritdoc />
    public ReviewApiService(
        IReviewReadingService readingService,
        IReviewCreatingService creatingService,
        IReviewUpdatingService updatingService,
        IReviewDeletingService deletingService,
        IMapper mapper)
    {
        this.readingService = readingService;
        this.creatingService = creatingService;
        this.updatingService = updatingService;
        this.deletingService = deletingService;
        this.mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<ListEnvelope<Review>> Get(ReviewsQuery query)
    {
        var (reviews, paging) = await readingService.Get(query, query.OnlyApproved);
        return new ListEnvelope<Review>(reviews.Select(mapper.Map<Review>), new Paging(paging));
    }

    /// <inheritdoc />
    public async Task<Envelope<Review>> Get(Guid id)
    {
        var review = await readingService.Get(id);
        return new Envelope<Review>(mapper.Map<Review>(review));
    }

    /// <inheritdoc />
    public async Task<Envelope<Review>> Create(Review review)
    {
        var createReview = mapper.Map<CreateReview>(review);
        var createdReview = await creatingService.Create(createReview);
        return new Envelope<Review>(mapper.Map<Review>(createdReview));
    }

    /// <inheritdoc />
    public async Task<Envelope<Review>> Update(Guid id, Review review)
    {
        var updateReview = mapper.Map<UpdateReview>(review);
        updateReview.ReviewId = id;
        var updatedReview = await updatingService.Update(updateReview);
        return new Envelope<Review>(mapper.Map<Review>(updatedReview));
    }

    /// <inheritdoc />
    public Task Delete(Guid id) => deletingService.Delete(id);
}