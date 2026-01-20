using System;
using System.Threading;
using System.Threading.Tasks;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Fora;

namespace DM.Web.API.Services.Fora;

/// <summary>
/// API service for forum topics
/// </summary>
public interface ITopicApiService
{
    /// <summary>
    /// Get forum topics
    /// </summary>
    /// <param name="forumId">Forum identifier</param>
    /// <param name="query">Search query</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Envelope of topics list</returns>
    Task<ListEnvelope<Topic>> Get(string forumId, TopicsQuery query, CancellationToken cancellationToken);

    /// <summary>
    /// Get topic
    /// </summary>
    /// <param name="topicId">Topic identifier</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Envelope of topic</returns>
    Task<Envelope<Topic>> Get(Guid topicId, CancellationToken cancellationToken);

    /// <summary>
    /// Create new topic
    /// </summary>
    /// <param name="forumId">Forum identifier</param>
    /// <param name="topic">Topic model</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Envelope of created topic</returns>
    Task<Envelope<Topic>> Create(string forumId, Topic topic, CancellationToken cancellationToken);

    /// <summary>
    /// Updates topic
    /// </summary>
    /// <param name="topicId">Topic identifier</param>
    /// <param name="topic">Topic model</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Envelop of updated topic</returns>
    Task<Envelope<Topic>> Update(Guid topicId, Topic topic, CancellationToken cancellationToken);

    /// <summary>
    /// Removes topic
    /// </summary>
    /// <param name="topicId">Topic identifier</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task Delete(Guid topicId, CancellationToken cancellationToken);
}