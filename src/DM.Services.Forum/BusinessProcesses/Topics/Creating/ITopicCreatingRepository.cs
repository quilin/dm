using System.Threading.Tasks;
using DM.Services.DataAccess.BusinessObjects.Fora;
using DM.Services.Forum.Dto.Output;

namespace DM.Services.Forum.BusinessProcesses.Topics.Creating;

/// <summary>
/// Creating topics storage
/// </summary>
internal interface ITopicCreatingRepository
{
    /// <summary>
    /// Create new topic
    /// </summary>
    /// <param name="forumTopic">DAL model</param>
    /// <returns>DTO model of created topic</returns>
    Task<Topic> Create(ForumTopic forumTopic);
}