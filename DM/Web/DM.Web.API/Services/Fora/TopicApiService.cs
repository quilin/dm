using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DM.Services.Forum.BusinessProcesses.Commentaries;
using DM.Services.Forum.BusinessProcesses.Topics;
using DM.Services.Forum.Dto;
using DM.Web.API.Dto.Common;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Fora;
using Topic = DM.Web.API.Dto.Fora.Topic;

namespace DM.Web.API.Services.Fora
{
    /// <inheritdoc />
    public class TopicApiService : ITopicApiService
    {
        private readonly ITopicReadingService topicReadingService;
        private readonly ITopicCreatingService topicCreatingService;
        private readonly ITopicUpdatingService topicUpdatingService;
        private readonly ITopicDeletingService topicDeletingService;
        private readonly ICommentaryReadingService commentaryReadingService;
        private readonly IMapper mapper;

        /// <inheritdoc />
        public TopicApiService(
            ITopicReadingService topicReadingService,
            ITopicCreatingService topicCreatingService,
            ITopicUpdatingService topicUpdatingService,
            ITopicDeletingService topicDeletingService,
            ICommentaryReadingService commentaryReadingService,
            IMapper mapper)
        {
            this.topicReadingService = topicReadingService;
            this.topicCreatingService = topicCreatingService;
            this.topicUpdatingService = topicUpdatingService;
            this.topicDeletingService = topicDeletingService;
            this.commentaryReadingService = commentaryReadingService;
            this.mapper = mapper;
        }

        /// <inheritdoc />
        public async Task<ListEnvelope<Topic>> Get(string forumId, TopicFilters filters, int entityNumber)
        {
            var (topics, paging) = filters.Attached
                ? (await topicReadingService.GetAttachedTopics(forumId), null)
                : await topicReadingService.GetTopicsList(forumId, entityNumber);
            return new ListEnvelope<Topic>(topics.Select(mapper.Map<Topic>),
                paging == null ? null : new Paging(paging));
        }

        /// <inheritdoc />
        public async Task<Envelope<Topic>> Get(Guid topicId)
        {
            var topic = await topicReadingService.GetTopic(topicId);
            return new Envelope<Topic>(mapper.Map<Topic>(topic));
        }

        /// <inheritdoc />
        public async Task<Envelope<Topic>> Create(string forumId, Topic topic)
        {
            var createTopic = mapper.Map<CreateTopic>(topic);
            createTopic.ForumTitle = forumId;
            var createdTopic = await topicCreatingService.CreateTopic(createTopic);
            return new Envelope<Topic>(mapper.Map<Topic>(createdTopic));
        }

        /// <inheritdoc />
        public async Task<Envelope<Topic>> Update(Guid topicId, Topic topic)
        {
            var updateTopic = mapper.Map<UpdateTopic>(topic);
            updateTopic.TopicId = topicId;
            var updatedTopic = await topicUpdatingService.UpdateTopic(updateTopic);
            return new Envelope<Topic>(mapper.Map<Topic>(updatedTopic));
        }

        /// <inheritdoc />
        public Task Delete(Guid topicId) => topicDeletingService.DeleteTopic(topicId);

        /// <inheritdoc />
        public async Task<ListEnvelope<Comment>> Get(Guid topicId, int entityNumber)
        {
            var (comments, paging) = await commentaryReadingService.GetCommentsList(topicId, entityNumber);
            return new ListEnvelope<Comment>(comments.Select(mapper.Map<Comment>), new Paging(paging));
        }
    }
}