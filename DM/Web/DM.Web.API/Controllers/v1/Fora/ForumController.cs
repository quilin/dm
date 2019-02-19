using System.Threading.Tasks;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Fora;
using DM.Web.API.Dto.Users;
using DM.Web.API.Services.Fora;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.API.Controllers.v1.Fora
{
    [Route("v1/fora")]
    public class ForumController : Controller
    {
        private readonly IForumApiService forumApiService;
        private readonly ITopicApiService topicApiService;
        private readonly IModeratorsApiService moderatorsApiService;

        public ForumController(
            IForumApiService forumApiService,
            ITopicApiService topicApiService,
            IModeratorsApiService moderatorsApiService)
        {
            this.forumApiService = forumApiService;
            this.topicApiService = topicApiService;
            this.moderatorsApiService = moderatorsApiService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ListEnvelope<Forum>), 200)]
        public Task<ListEnvelope<Forum>> GetFora() => forumApiService.Get();

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Envelope<Forum>), 200)]
        [ProducesResponseType(typeof(GeneralError), 404)]
        public Task<Envelope<Forum>> GetForum(string id) => forumApiService.Get(id);

        [HttpGet("{id}/moderators")]
        [ProducesResponseType(typeof(ListEnvelope<User>), 200)]
        [ProducesResponseType(typeof(GeneralError), 404)]
        public Task<ListEnvelope<User>> GetModerators(string id) => moderatorsApiService.GetModerators(id);

        [HttpGet("{id}/topics")]
        [ProducesResponseType(typeof(ListEnvelope<Topic>), 200)]
        [ProducesResponseType(typeof(BadRequestError), 400)]
        [ProducesResponseType(typeof(GeneralError), 404)]
        public Task<ListEnvelope<Topic>> GetTopics(string id, [FromQuery] TopicFilters f, [FromQuery] int n = 1) =>
            topicApiService.Get(id, f, n);

        [HttpPost("{id}/topics")]
        [ProducesResponseType(typeof(Envelope<Topic>), 201)]
        [ProducesResponseType(typeof(BadRequestError), 400)]
        [ProducesResponseType(typeof(GeneralError), 403)]
        [ProducesResponseType(typeof(GeneralError), 404)]
        public Task<Envelope<Topic>> PostTopic(string id, [FromBody] Topic topic) => topicApiService.Create(id, topic);
    }
}