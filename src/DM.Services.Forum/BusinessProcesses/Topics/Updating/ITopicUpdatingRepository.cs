using System.Threading;
using System.Threading.Tasks;
using DM.Services.DataAccess.BusinessObjects.Fora;
using DM.Services.DataAccess.RelationalStorage;
using DM.Services.Forum.Dto.Output;

namespace DM.Services.Forum.BusinessProcesses.Topics.Updating;

/// <summary>
/// Updating topics storage
/// </summary>
public interface ITopicUpdatingRepository
{
    /// <summary>
    /// Update existing topic
    /// </summary>
    /// <param name="updateBuilder"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>DTO model of updated topic</returns>
    Task<Topic> Update(IUpdateBuilder<ForumTopic> updateBuilder, CancellationToken cancellationToken);
}