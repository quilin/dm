using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Services.Common.Dto;
using DM.Services.Core.Dto;
using DM.Services.Forum.Dto;

namespace DM.Services.Forum.Implementation
{
    /// <summary>
    /// Forum service client endpoint
    /// </summary>
    public interface IForumService
    {
        /// <summary>
        /// Get list of available fora
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Dto.Forum>> GetForaList();

        /// <summary>
        /// Get available forum by title
        /// </summary>
        /// <param name="forumTitle">Forum title</param>
        /// <returns></returns>
        Task<Dto.Forum> GetForum(string forumTitle);

        /// <summary>
        /// Get list of forum moderators by forum title
        /// </summary>
        /// <param name="forumTitle">Forum title</param>
        /// <returns></returns>
        Task<IEnumerable<GeneralUser>> GetModerators(string forumTitle);

        /// <summary>
        /// Get topics page of certain forum by its title
        /// </summary>
        /// <param name="forumTitle">Forum title</param>
        /// <param name="entityNumber">Number of entity that page must contain</param>
        /// <returns>Pair of topics list and paging data</returns>
        Task<(IEnumerable<Topic> topics, PagingData paging)> GetTopicsList(string forumTitle, int entityNumber);

        /// <summary>
        /// Get attached topics of certain forum by its title
        /// </summary>
        /// <param name="forumTitle">Forum title</param>
        /// <returns></returns>
        Task<IEnumerable<Topic>> GetAttachedTopics(string forumTitle);

        /// <summary>
        /// Get topic by id
        /// </summary>
        /// <param name="topicId">Topic id</param>
        /// <returns></returns>
        Task<Topic> GetTopic(Guid topicId);

        /// <summary>
        /// Create new topic
        /// </summary>
        /// <param name="createTopic">Create topic model</param>
        /// <returns></returns>
        Task<Topic> CreateTopic(CreateTopic createTopic);

        /// <summary>
        /// Update existing topic
        /// </summary>
        /// <param name="updateTopic">Update topic model</param>
        /// <returns></returns>
        Task<Topic> UpdateTopic(UpdateTopic updateTopic);

        /// <summary>
        /// Remove existing topic
        /// </summary>
        /// <param name="topicId">Topic identifier</param>
        /// <returns></returns>
        Task RemoveTopic(Guid topicId);

        /// <summary>
        /// Get list of topic comments by topic id
        /// </summary>
        /// <param name="topicId">Topic identifier</param>
        /// <param name="entityNumber">Number of entity that page must contain</param>
        /// <returns>Pair of comments list and paging data</returns>
        Task<(IEnumerable<Comment> comments, PagingData paging)> GetCommentsList(Guid topicId, int entityNumber);

        /// <summary>
        /// Create new like from current user to selected topic
        /// </summary>
        /// <param name="topicId">Topic identifier</param>
        /// <returns>User who liked the topic</returns>
        Task<GeneralUser> LikeTopic(Guid topicId);

        /// <summary>
        /// Remove existing like from current user to selected topic
        /// </summary>
        /// <param name="topicId">Topic identifier</param>
        /// <returns></returns>
        Task DislikeTopic(Guid topicId);
    }
}