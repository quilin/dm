using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Services.Core.Dto;

namespace DM.Services.Community.BusinessProcesses.Reviews.Reading
{
    /// <summary>
    /// Service for reading user reviews
    /// </summary>
    public interface IReviewReadingService
    {
        /// <summary>
        /// Get reviews by query
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<(IEnumerable<Review> reviews, PagingResult paging)> Get(PagingQuery query);

        /// <summary>
        /// Get random approved review
        /// </summary>
        /// <returns></returns>
        Task<Review> GetRandom();

        /// <summary>
        /// Get single review
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Review> Get(Guid id);
    }
}