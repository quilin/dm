using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DM.Services.Community.BusinessProcesses.Reviews.Reading;
using DM.Services.DataAccess;
using DM.Services.DataAccess.RelationalStorage;
using Microsoft.EntityFrameworkCore;

namespace DM.Services.Community.BusinessProcesses.Reviews.Updating;

/// <inheritdoc />
internal class ReviewUpdatingRepository : IReviewUpdatingRepository
{
    private readonly DmDbContext dbContext;
    private readonly IMapper mapper;

    /// <inheritdoc />
    public ReviewUpdatingRepository(
        DmDbContext dbContext,
        IMapper mapper)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }
        
    /// <inheritdoc />
    public async Task<Review> Update(IUpdateBuilder<DataAccess.BusinessObjects.Common.Review> updateReview)
    {
        var reviewId = updateReview.AttachTo(dbContext);
        await dbContext.SaveChangesAsync();
        return await dbContext.Reviews
            .Where(r => r.ReviewId == reviewId)
            .ProjectTo<Review>(mapper.ConfigurationProvider)
            .FirstAsync();
    }
}