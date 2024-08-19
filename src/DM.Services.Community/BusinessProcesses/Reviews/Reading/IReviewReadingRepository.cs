using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DM.Services.Core.Dto;

namespace DM.Services.Community.BusinessProcesses.Reviews.Reading;

/// <summary>
/// Storage for reading the site reviews
/// </summary>
public interface IReviewReadingRepository
{
    /// <summary>
    /// Get total count of reviews
    /// </summary>
    /// <param name="approvedOnly">Only count approved reviews</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<int> Count(bool approvedOnly, CancellationToken cancellationToken);

    /// <summary>
    /// Get reviews list
    /// </summary>
    /// <param name="paging">Paging data</param>
    /// <param name="approvedOnly">Only return approved reviews</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IEnumerable<Review>> Get(PagingData paging, bool approvedOnly, CancellationToken cancellationToken);

    /// <summary>
    /// Get single review
    /// </summary>
    /// <param name="id">Review identifier</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Review> Get(Guid id, CancellationToken cancellationToken);
}