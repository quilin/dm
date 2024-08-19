using AutoMapper;
using AutoMapper.QueryableExtensions;
using DM.Services.Community.BusinessProcesses.Reviews.Reading;
using DM.Services.Community.BusinessProcesses.Reviews.Updating;
using DM.Services.DataAccess;
using DM.Services.DataAccess.RelationalStorage;
using Microsoft.EntityFrameworkCore;

namespace DM.Services.Community.Storage.Storages.Reviews;

/// <inheritdoc />
internal class ReviewUpdatingRepository(
    DmDbContext dbContext,
    IMapper mapper) : IReviewUpdatingRepository
{
    /// <inheritdoc />
    public async Task<Review> Update(IUpdateBuilder<DataAccess.BusinessObjects.Common.Review> updateReview,
        CancellationToken cancellationToken)
    {
        var reviewId = updateReview.AttachTo(dbContext);
        await dbContext.SaveChangesAsync(cancellationToken);
        return await dbContext.Reviews
            .Where(r => r.ReviewId == reviewId)
            .ProjectTo<Review>(mapper.ConfigurationProvider)
            .FirstAsync(cancellationToken);
    }
}