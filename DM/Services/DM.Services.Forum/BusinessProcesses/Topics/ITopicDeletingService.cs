using System;
using System.Threading.Tasks;

namespace DM.Services.Forum.BusinessProcesses.Topics
{
    /// <summary>
    /// Service to delete topics
    /// </summary>
    public interface ITopicDeletingService
    {
        /// <summary>
        /// Remove existing topic
        /// </summary>
        /// <param name="topicId">Topic identifier</param>
        /// <returns></returns>
        Task DeleteTopic(Guid topicId);
    }
}