using System.Threading;
using System.Threading.Tasks;
using DM.Services.Forum.Dto.Input;
using DM.Services.Forum.Dto.Output;

namespace DM.Services.Forum.BusinessProcesses.Topics.Creating;

/// <summary>
/// Service for creating the topic
/// </summary>
public interface ITopicCreatingService
{
    /// <summary>
    /// Create new topic
    /// </summary>
    /// <param name="createTopic">Create topic model</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Topic> CreateTopic(CreateTopic createTopic, CancellationToken cancellationToken);
}