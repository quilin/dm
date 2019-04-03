using System;
using DM.Services.DataAccess.BusinessObjects.Fora;
using DM.Services.Forum.Dto;

namespace DM.Services.Forum.Factories
{
    /// <summary>
    /// Factory for topic DAL model
    /// </summary>
    public interface ITopicFactory
    {
        /// <summary>
        /// Create topic DAL for topic creation
        /// </summary>
        /// <param name="forumId">Forum identifier</param>
        /// <param name="createTopic">Topic DTO</param>
        /// <returns></returns>
        ForumTopic Create(Guid forumId, CreateTopic createTopic);
    }
}