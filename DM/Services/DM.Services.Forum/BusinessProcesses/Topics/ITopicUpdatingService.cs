using System.Threading.Tasks;
using DM.Services.Forum.Dto;

namespace DM.Services.Forum.BusinessProcesses.Topics
{
    /// <summary>
    /// Service for updating topics
    /// </summary>
    public interface ITopicUpdatingService
    {
        /// <summary>
        /// Update existing topic
        /// </summary>
        /// <param name="updateTopic">Update topic model</param>
        /// <returns></returns>
        Task<Topic> UpdateTopic(UpdateTopic updateTopic);
    }
}