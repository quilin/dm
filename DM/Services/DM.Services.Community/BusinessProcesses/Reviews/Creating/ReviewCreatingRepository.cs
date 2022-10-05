using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DM.Services.Community.BusinessProcesses.Reviews.Reading;
using DM.Services.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace DM.Services.Community.BusinessProcesses.Reviews.Creating;

/// <inheritdoc />
internal class ReviewCreatingRepository : IReviewCreatingRepository
{
    private readonly DmDbContext dbContext;
    private readonly IMapper mapper;

    /// <inheritdoc />
    public ReviewCreatingRepository(
        DmDbContext dbContext,
        IMapper mapper)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }
        
    /// <inheritdoc />
    public async Task<Review> Create(DataAccess.BusinessObjects.Common.Review review)
    {
        dbContext.Reviews.Add(review);
        await dbContext.SaveChangesAsync();
        return await dbContext.Reviews
            .Where(r => r.ReviewId == review.ReviewId)
            .ProjectTo<Review>(mapper.ConfigurationProvider)
            .FirstAsync();
    }
}