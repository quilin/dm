using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DM.Services.Core.Dto;
using DM.Services.Core.Extensions;
using DM.Services.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace DM.Services.Community.BusinessProcesses.Reviews.Reading;

/// <inheritdoc />
internal class ReviewReadingRepository : IReviewReadingRepository
{
    private readonly DmDbContext dbContext;
    private readonly IMapper mapper;

    /// <inheritdoc />
    public ReviewReadingRepository(
        DmDbContext dbContext,
        IMapper mapper)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }

    /// <inheritdoc />
    public Task<int> Count(bool approvedOnly) => dbContext.Reviews
        .Where(r => (!approvedOnly || r.IsApproved) && !r.IsRemoved)
        .CountAsync();

    /// <inheritdoc />
    public async Task<IEnumerable<Review>> Get(PagingData paging, bool approvedOnly) => await dbContext.Reviews
        .Where(r => (!approvedOnly || r.IsApproved) && !r.IsRemoved)
        .OrderByDescending(r => r.CreateDate)
        .Page(paging)
        .ProjectTo<Review>(mapper.ConfigurationProvider)
        .ToArrayAsync();

    /// <inheritdoc />
    public Task<Review> Get(Guid id) => dbContext.Reviews
        .Where(r => !r.IsRemoved && r.ReviewId == id)
        .ProjectTo<Review>(mapper.ConfigurationProvider)
        .FirstOrDefaultAsync();
}