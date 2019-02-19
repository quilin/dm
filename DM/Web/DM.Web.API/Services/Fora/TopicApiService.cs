using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DM.Services.Forum.Implementation;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Fora;

namespace DM.Web.API.Services.Fora
{
    public class TopicApiService : ITopicApiService
    {
        private readonly IForumService forumService;
        private readonly IMapper mapper;

        public TopicApiService(
            IForumService forumService,
            IMapper mapper)
        {
            this.forumService = forumService;
            this.mapper = mapper;
        }
        
        public async Task<ListEnvelope<Topic>> Get(string forumId, TopicFilters filters, int entityNumber)
        {
            var (topics, paging) = filters.Attached
                ? (await forumService.GetAttachedTopics(forumId), null)
                : await forumService.GetTopicsList(forumId, entityNumber);
            return new ListEnvelope<Topic>(topics.Select(mapper.Map<Topic>),
                paging == null ? null : new Paging(paging));
        }

        public Task<Envelope<Topic>> Get(Guid topicId)
        {
            throw new NotImplementedException();
        }
    }
}