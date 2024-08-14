using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Services.Core.Dto;

namespace DM.Services.Community.BusinessProcesses.Reviews.Reading;

/// <summary>
/// Storage for reading the site reviews
/// </summary>
internal interface IReviewReadingRepository
{
    /// <summary>
    /// Get total count of reviews
    /// </summary>
    /// <param name="approvedOnly">Only count approved reviews</param>
    /// <returns></returns>
    Task<int> Count(bool approvedOnly);

    /// <summary>
    /// Get reviews list
    /// </summary>
    /// <param name="paging">Paging data</param>
    /// <param name="approvedOnly">Only return approved reviews</param>
    /// <returns></returns>
    Task<IEnumerable<Review>> Get(PagingData paging, bool approvedOnly);

    /// <summary>
    /// Get single review
    /// </summary>
    /// <param name="id">Review identifier</param>
    /// <returns></returns>
    Task<Review> Get(Guid id);
}