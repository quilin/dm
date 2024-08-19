using System;
using System.Threading;
using System.Threading.Tasks;

namespace DM.Services.Forum.BusinessProcesses.Topics.Deleting;

/// <summary>
/// Service to delete topics
/// </summary>
public interface ITopicDeletingService
{
    /// <summary>
    /// Remove existing topic
    /// </summary>
    /// <param name="topicId">Topic identifier</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task DeleteTopic(Guid topicId, CancellationToken cancellationToken);
}