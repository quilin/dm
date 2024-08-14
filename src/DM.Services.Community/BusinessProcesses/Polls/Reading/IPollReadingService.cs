using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Services.Core.Dto;

namespace DM.Services.Community.BusinessProcesses.Polls.Reading;

/// <summary>
/// Service for polls reading
/// </summary>
public interface IPollReadingService
{
    /// <summary>
    /// Get list of polls
    /// </summary>
    /// <param name="pagingQuery">Paging query</param>
    /// <param name="onlyActive">Only get active polls</param>
    /// <returns></returns>
    Task<(IEnumerable<Poll> polls, PagingResult paging)> Get(PagingQuery pagingQuery, bool onlyActive);

    /// <summary>
    /// Get single poll
    /// </summary>
    /// <param name="pollId">Poll identifier</param>
    /// <returns></returns>
    Task<Poll> Get(Guid pollId);
}