using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Services.Core.Dto;

namespace DM.Services.Community.BusinessProcesses.Polls.Reading
{
    /// <summary>
    /// Storage for reading polls
    /// </summary>
    public interface IPollReadingRepository
    {
        /// <summary>
        /// Count all
        /// </summary>
        /// <param name="activeUntil"></param>
        /// <returns></returns>
        Task<long> Count(DateTimeOffset? activeUntil);

        /// <summary>
        /// Get list of polls
        /// </summary>
        /// <param name="activeUntil"></param>
        /// <param name="pagingData"></param>
        /// <returns></returns>
        Task<IEnumerable<Poll>> Get(DateTimeOffset? activeUntil, PagingData pagingData);

        /// <summary>
        /// Get single poll by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Poll> Get(Guid id);
    }
}