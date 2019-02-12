using System;
using System.Linq;
using System.Threading.Tasks;
using DM.Services.Forum.Implementation;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Fora;

namespace DM.Web.API.Services.Fora
{
    public class TopicApiService : ITopicApiService
    {
        private readonly IForumService forumService;

        public TopicApiService(
            IForumService forumService)
        {
            this.forumService = forumService;
        }
        
        public async Task<ListEnvelope<Topic>> Get(string forumId, TopicFilters filters, int entityNumber)
        {
            var (topics, paging) = filters.Attached
                ? (await forumService.GetAttachedTopics(forumId), null)
                : await forumService.GetTopicsList(forumId, entityNumber);
            return new ListEnvelope<Topic>(topics.Select(t => new Topic(t)),
                paging == null ? null : new Paging(paging));
        }

        public Task<Envelope<Topic>> Get(Guid topicId)
        {
            throw new NotImplementedException();
        }
    }
}