using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DM.Services.Core.Dto;
using DM.Services.Core.Extensions;
using DM.Services.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace DM.Services.Community.BusinessProcesses.Reviews.Reading;

/// <inheritdoc />
internal class ReviewReadingRepository(
    DmDbContext dbContext,
    IMapper mapper) : IReviewReadingRepository
{
    /// <inheritdoc />
    public Task<int> Count(bool approvedOnly, CancellationToken cancellationToken) =>
        dbContext.Reviews
            .Where(r => (!approvedOnly || r.IsApproved) && !r.IsRemoved)
            .CountAsync(cancellationToken);

    /// <inheritdoc />
    public async Task<IEnumerable<Review>> Get(PagingData paging, bool approvedOnly,
        CancellationToken cancellationToken) =>
        await dbContext.Reviews
            .Where(r => (!approvedOnly || r.IsApproved) && !r.IsRemoved)
            .OrderByDescending(r => r.CreateDate)
            .Page(paging)
            .ProjectTo<Review>(mapper.ConfigurationProvider)
            .ToArrayAsync(cancellationToken);

    /// <inheritdoc />
    public Task<Review> Get(Guid id, CancellationToken cancellationToken) =>
        dbContext.Reviews
            .Where(r => !r.IsRemoved && r.ReviewId == id)
            .ProjectTo<Review>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);
}