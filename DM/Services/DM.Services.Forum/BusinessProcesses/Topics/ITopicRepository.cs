using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Services.Core.Dto;
using DM.Services.Core.Dto.Enums;
using DM.Services.DataAccess.BusinessObjects.Fora;
using DM.Services.DataAccess.RelationalStorage;
using DM.Services.Forum.Dto.Output;

namespace DM.Services.Forum.BusinessProcesses.Topics
{
    /// <summary>
    /// Forum topics storage
    /// </summary>
    public interface ITopicRepository
    {
        /// <summary>
        /// Get number of forum topics
        /// </summary>
        /// <param name="forumId">Forum identifier</param>
        /// <returns></returns>
        Task<int> Count(Guid forumId);

        /// <summary>
        /// Get list of forum topics
        /// </summary>
        /// <param name="forumId">Forum identifier</param>
        /// <param name="pagingData">Paging data</param>
        /// <param name="attached">Select attached/not attached topics exclusively</param>
        /// <returns></returns>
        Task<IEnumerable<Topic>> Get(Guid forumId, PagingData pagingData, bool attached);

        /// <summary>
        /// Get topic
        /// </summary>
        /// <param name="topicId">Topic identifier</param>
        /// <param name="accessPolicy">Forum access policy</param>
        /// <returns></returns>
        Task<Topic> Get(Guid topicId, ForumAccessPolicy accessPolicy);

        /// <summary>
        /// Create new topic
        /// </summary>
        /// <param name="forumTopic">DAL model</param>
        /// <returns>DTO model of created topic</returns>
        Task<Topic> Create(ForumTopic forumTopic);

        /// <summary>
        /// Update existing topic
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updateBuilder"></param>
        /// <returns>DTO model of updated topic</returns>
        Task<Topic> Update(Guid id, UpdateBuilder<ForumTopic> updateBuilder);
    }
}