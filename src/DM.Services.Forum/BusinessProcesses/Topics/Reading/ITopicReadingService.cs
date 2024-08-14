using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Services.Core.Dto;
using DM.Services.Forum.Dto.Output;

namespace DM.Services.Forum.BusinessProcesses.Topics.Reading;

/// <summary>
/// Service for reading forum topics
/// </summary>
public interface ITopicReadingService
{
    /// <summary>
    /// Get topics page of certain forum by its title
    /// </summary>
    /// <param name="forumTitle">Forum title</param>
    /// <param name="query">Paging query</param>
    /// <returns>Pair of topics list and paging data</returns>
    Task<(IEnumerable<Topic> topics, PagingResult paging)> GetTopicsList(string forumTitle, PagingQuery query);

    /// <summary>
    /// Get attached topics of certain forum by its title
    /// </summary>
    /// <param name="forumTitle">Forum title</param>
    /// <returns></returns>
    Task<IEnumerable<Topic>> GetAttachedTopics(string forumTitle);

    /// <summary>
    /// Get topic by id
    /// </summary>
    /// <param name="topicId">Topic identifier</param>
    /// <returns></returns>
    Task<Topic> GetTopic(Guid topicId);
}