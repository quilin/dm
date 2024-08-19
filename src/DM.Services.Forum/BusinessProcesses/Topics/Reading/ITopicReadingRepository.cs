using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DM.Services.Core.Dto;
using DM.Services.Core.Dto.Enums;
using DM.Services.Forum.Dto.Output;

namespace DM.Services.Forum.BusinessProcesses.Topics.Reading;

/// <summary>
/// Forum topics storage
/// </summary>
public interface ITopicReadingRepository
{
    /// <summary>
    /// Get number of forum topics
    /// </summary>
    /// <param name="forumId">Forum identifier</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<int> Count(Guid forumId, CancellationToken cancellationToken);

    /// <summary>
    /// Get list of forum topics
    /// </summary>
    /// <param name="forumId">Forum identifier</param>
    /// <param name="pagingData">Paging data</param>
    /// <param name="attached">Select attached/not attached topics exclusively</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IEnumerable<Topic>> Get(Guid forumId, PagingData pagingData, bool attached,
        CancellationToken cancellationToken);

    /// <summary>
    /// Get topic
    /// </summary>
    /// <param name="topicId">Topic identifier</param>
    /// <param name="accessPolicy">Forum access policy</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Topic> Get(Guid topicId, ForumAccessPolicy accessPolicy, CancellationToken cancellationToken);
}