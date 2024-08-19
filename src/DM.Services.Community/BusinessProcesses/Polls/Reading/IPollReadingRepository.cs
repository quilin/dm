using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DM.Services.Core.Dto;

namespace DM.Services.Community.BusinessProcesses.Polls.Reading;

/// <summary>
/// Storage for reading polls
/// </summary>
public interface IPollReadingRepository
{
    /// <summary>
    /// Count all
    /// </summary>
    /// <param name="activeAt"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<long> Count(DateTimeOffset? activeAt, CancellationToken cancellationToken);

    /// <summary>
    /// Get list of polls
    /// </summary>
    /// <param name="activeAt"></param>
    /// <param name="pagingData"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IEnumerable<Poll>> Get(DateTimeOffset? activeAt, PagingData pagingData, CancellationToken cancellationToken);

    /// <summary>
    /// Get single poll by id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Poll> Get(Guid id, CancellationToken cancellationToken);
}