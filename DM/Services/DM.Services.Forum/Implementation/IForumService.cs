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
        /// <param name="createTopic"></param>
        /// <returns></returns>
        Task<Topic> CreateTopic(CreateTopic createTopic);

        /// <summary>
        /// Get list of topic comments by topic id
        /// </summary>
        /// <param name="topicId">Topic id</param>
        /// <param name="entityNumber">Number of entity that page must contain</param>
        /// <returns>Pair of comments list and paging data</returns>
        Task<(IEnumerable<Comment> comments, PagingData paging)> GetCommentsList(Guid topicId, int entityNumber);
    }
}