using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DM.Services.Forum.Dto;
using DM.Services.Forum.Implementation;
using DM.Web.API.Dto.Common;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Fora;
using Topic = DM.Web.API.Dto.Fora.Topic;

namespace DM.Web.API.Services.Fora
{
    /// <inheritdoc />
    public class TopicApiService : ITopicApiService
    {
        private readonly IForumService forumService;
        private readonly IMapper mapper;

        /// <inheritdoc />
        public TopicApiService(
            IForumService forumService,
            IMapper mapper)
        {
            this.forumService = forumService;
            this.mapper = mapper;
        }

        /// <inheritdoc />
        public async Task<ListEnvelope<Topic>> Get(string forumId, TopicFilters filters, int entityNumber)
        {
            var (topics, paging) = filters.Attached
                ? (await forumService.GetAttachedTopics(forumId), null)
                : await forumService.GetTopicsList(forumId, entityNumber);
            return new ListEnvelope<Topic>(topics.Select(mapper.Map<Topic>),
                paging == null ? null : new Paging(paging));
        }

        /// <inheritdoc />
        public async Task<Envelope<Topic>> Get(Guid topicId)
        {
            var topic = await forumService.GetTopic(topicId);
            return new Envelope<Topic>(mapper.Map<Topic>(topic));
        }

        /// <inheritdoc />
        public async Task<Envelope<Topic>> Create(string forumId, Topic topic)
        {
            var createTopic = mapper.Map<CreateTopic>(topic);
            createTopic.ForumTitle = forumId;
            var createdTopic = await forumService.CreateTopic(createTopic);
            return new Envelope<Topic>(mapper.Map<Topic>(createdTopic));
        }

        /// <inheritdoc />
        public async Task<Envelope<Topic>> Update(Guid topicId, Topic topic)
        {
            var updateTopic = mapper.Map<UpdateTopic>(topic);
            updateTopic.TopicId = topicId;
            var updatedTopic = await forumService.UpdateTopic(updateTopic);
            return new Envelope<Topic>(mapper.Map<Topic>(updatedTopic));
        }

        /// <inheritdoc />
        public async Task<ListEnvelope<Comment>> Get(Guid topicId, int entityNumber)
        {
            var (comments, paging) = await forumService.GetCommentsList(topicId, entityNumber);
            return new ListEnvelope<Comment>(comments.Select(mapper.Map<Comment>), new Paging(paging));
        }
    }
}