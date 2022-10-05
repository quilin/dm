using System;
using DM.Services.Core.Implementation;
using DM.Services.DataAccess.BusinessObjects.Common;

namespace DM.Services.Community.BusinessProcesses.Reviews.Creating;

/// <inheritdoc />
internal class ReviewFactory : IReviewFactory
{
    private readonly IGuidFactory guidFactory;
    private readonly IDateTimeProvider dateTimeProvider;

    /// <inheritdoc />
    public ReviewFactory(
        IGuidFactory guidFactory,
        IDateTimeProvider dateTimeProvider)
    {
        this.guidFactory = guidFactory;
        this.dateTimeProvider = dateTimeProvider;
    }
        
    /// <inheritdoc />
    public Review Create(CreateReview createReview, Guid userId)
    {
        return new Review
        {
            ReviewId = guidFactory.Create(),
            UserId = userId,
            CreateDate = dateTimeProvider.Now,
            Text = createReview.Text.Trim(),
            IsApproved = false
        };
    }
}