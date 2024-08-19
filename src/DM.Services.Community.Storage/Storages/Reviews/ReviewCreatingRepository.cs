using AutoMapper;
using AutoMapper.QueryableExtensions;
using DM.Services.Community.BusinessProcesses.Reviews.Creating;
using DM.Services.Community.BusinessProcesses.Reviews.Reading;
using DM.Services.DataAccess;
using Microsoft.EntityFrameworkCore;
using DbReview = DM.Services.DataAccess.BusinessObjects.Common.Review;

namespace DM.Services.Community.Storage.Storages.Reviews;

/// <inheritdoc />
internal class ReviewCreatingRepository(
    DmDbContext dbContext,
    IMapper mapper) : IReviewCreatingRepository
{
    /// <inheritdoc />
    public async Task<Review> Create(DbReview review, CancellationToken cancellationToken)
    {
        dbContext.Reviews.Add(review);
        await dbContext.SaveChangesAsync(cancellationToken);
        return await dbContext.Reviews
            .Where(r => r.ReviewId == review.ReviewId)
            .ProjectTo<Review>(mapper.ConfigurationProvider)
            .FirstAsync(cancellationToken);
    }
}