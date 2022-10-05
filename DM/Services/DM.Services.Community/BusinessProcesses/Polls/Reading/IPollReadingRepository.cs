using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Services.Core.Dto;

namespace DM.Services.Community.BusinessProcesses.Polls.Reading;

/// <summary>
/// Storage for reading polls
/// </summary>
internal interface IPollReadingRepository
{
    /// <summary>
    /// Count all
    /// </summary>
    /// <param name="activeAt"></param>
    /// <returns></returns>
    Task<long> Count(DateTimeOffset? activeAt);

    /// <summary>
    /// Get list of polls
    /// </summary>
    /// <param name="activeAt"></param>
    /// <param name="pagingData"></param>
    /// <returns></returns>
    Task<IEnumerable<Poll>> Get(DateTimeOffset? activeAt, PagingData pagingData);

    /// <summary>
    /// Get single poll by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Poll> Get(Guid id);
}