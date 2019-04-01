using System;
using System.Threading.Tasks;
using DM.Web.API.Dto.Common;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Fora;

namespace DM.Web.API.Services.Fora
{
    /// <summary>
    /// API service for forum topics
    /// </summary>
    public interface ITopicApiService
    {
        /// <summary>
        /// Get forum topics
        /// </summary>
        /// <param name="forumId">Forum id</param>
        /// <param name="filters">Search filters</param>
        /// <param name="entityNumber">Entity number</param>
        /// <returns>Envelope of topics list</returns>
        Task<ListEnvelope<Topic>> Get(string forumId, TopicFilters filters, int entityNumber);

        /// <summary>
        /// Get topic
        /// </summary>
        /// <param name="topicId">Topic id</param>
        /// <returns>Envelope of topic</returns>
        Task<Envelope<Topic>> Get(Guid topicId);

        /// <summary>
        /// Create new topic
        /// </summary>
        /// <param name="forumId">Forum id</param>
        /// <param name="topic">Topic model</param>
        /// <returns>Envelope of topic</returns>
        Task<Envelope<Topic>> Create(string forumId, Topic topic);

        /// <summary>
        /// Get topics commentaries
        /// </summary>
        /// <param name="topicId">Topic id</param>
        /// <param name="entityNumber">Entity number</param>
        /// <returns>Envelope of commentaries list</returns>
        Task<ListEnvelope<Comment>> Get(Guid topicId, int entityNumber);
    }
}