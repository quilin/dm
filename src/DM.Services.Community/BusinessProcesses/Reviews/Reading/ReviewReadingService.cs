using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Common.Authorization;
using DM.Services.Core.Dto;
using DM.Services.Core.Exceptions;

namespace DM.Services.Community.BusinessProcesses.Reviews.Reading;

/// <inheritdoc />
internal class ReviewReadingService : IReviewReadingService
{
    private readonly IIntentionManager intentionManager;
    private readonly IReviewReadingRepository repository;
    private readonly IIdentityProvider identityProvider;

    /// <inheritdoc />
    public ReviewReadingService(
        IIntentionManager intentionManager,
        IReviewReadingRepository repository,
        IIdentityProvider identityProvider)
    {
        this.intentionManager = intentionManager;
        this.repository = repository;
        this.identityProvider = identityProvider;
    }

    /// <inheritdoc />
    public async Task<(IEnumerable<Review> reviews, PagingResult paging)> Get(PagingQuery query, bool onlyApproved)
    {
        onlyApproved = onlyApproved || !intentionManager.IsAllowed(ReviewIntention.ReadUnapproved);
        var totalCount = await repository.Count(onlyApproved);
        var pagingData = new PagingData(query,
            identityProvider.Current.Settings.Paging.EntitiesPerPage, totalCount);

        var reviews = await repository.Get(pagingData, onlyApproved);
        return (reviews, pagingData.Result);
    }

    /// <inheritdoc />
    public async Task<Review> Get(Guid id)
    {
        var review = await repository.Get(id);
        if (review == null || !review.Approved && !intentionManager.IsAllowed(ReviewIntention.ReadUnapproved))
        {
            throw new HttpException(HttpStatusCode.Gone, "Review not found");
        }

        return review;
    }
}