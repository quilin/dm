using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DM.Services.Core.Dto;

namespace DM.Services.Community.BusinessProcesses.Reviews.Reading;

/// <summary>
/// Service for reading user reviews
/// </summary>
public interface IReviewReadingService
{
    /// <summary>
    /// Get reviews by query
    /// </summary>
    /// <param name="query"></param>
    /// <param name="onlyApproved"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<(IEnumerable<Review> reviews, PagingResult paging)> Get(PagingQuery query, bool onlyApproved,
        CancellationToken cancellationToken);

    /// <summary>
    /// Get single review
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Review> Get(Guid id, CancellationToken cancellationToken);
}