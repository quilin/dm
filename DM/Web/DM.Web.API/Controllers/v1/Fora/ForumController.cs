using System.Threading.Tasks;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Fora;
using DM.Web.API.Dto.Users;
using DM.Web.API.Services.Fora;
using DM.Web.Core.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.API.Controllers.v1.Fora
{
    /// <summary>
    /// Fora
    /// </summary>
    [Route("v1/fora")]
    public class ForumController : Controller
    {
        private readonly IForumApiService forumApiService;
        private readonly ITopicApiService topicApiService;
        private readonly IModeratorsApiService moderatorsApiService;

        /// <inheritdoc />
        public ForumController(
            IForumApiService forumApiService,
            ITopicApiService topicApiService,
            IModeratorsApiService moderatorsApiService)
        {
            this.forumApiService = forumApiService;
            this.topicApiService = topicApiService;
            this.moderatorsApiService = moderatorsApiService;
        }

        /// <summary>
        /// Get list of available fora
        /// </summary>
        /// <response code="200"></response>
        [HttpGet]
        [ProducesResponseType(typeof(ListEnvelope<Forum>), 200)]
        public Task<ListEnvelope<Forum>> GetFora() => forumApiService.Get();

        /// <summary>
        /// Get certain forum
        /// </summary>
        /// <param name="id"></param>
        /// <response code="200"></response>
        /// <response code="404">No available forum with this id</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Envelope<Forum>), 200)]
        [ProducesResponseType(typeof(GeneralError), 404)]
        public Task<Envelope<Forum>> GetForum(string id) => forumApiService.Get(id);

        /// <summary>
        /// Get forum moderators
        /// </summary>
        /// <param name="id"></param>
        /// <response code="200"></response>
        /// <response code="404">No available forum with this id</response>
        [HttpGet("{id}/moderators")]
        [ProducesResponseType(typeof(ListEnvelope<User>), 200)]
        [ProducesResponseType(typeof(GeneralError), 404)]
        public Task<ListEnvelope<User>> GetModerators(string id) => moderatorsApiService.GetModerators(id);

        /// <summary>
        /// Get list of forum topics
        /// </summary>
        /// <param name="id"></param>
        /// <param name="f">Filters</param>
        /// <param name="n">Entity number</param>
        /// <response code="200"></response>
        /// <response code="400">Some properties of the passed search parameters were invalid</response>
        /// <response code="404">No available forum with this id</response>
        [HttpGet("{id}/topics")]
        [ProducesResponseType(typeof(ListEnvelope<Topic>), 200)]
        [ProducesResponseType(typeof(BadRequestError), 400)]
        [ProducesResponseType(typeof(GeneralError), 404)]
        public Task<ListEnvelope<Topic>> GetTopics(string id, [FromQuery] TopicFilters f, [FromQuery] int n = 1) =>
            topicApiService.Get(id, f, n);

        /// <summary>
        /// Post new topic
        /// </summary>
        /// <param name="id"></param>
        /// <param name="topic"></param>
        /// <response code="201"></response>
        /// <response code="400">Some of the passed topic properties were invalid</response>
        /// <response code="401">User must be authenticated</response>
        /// <response code="403">User is not allowed to create topics in this forum</response>
        /// <response code="404">No available forum with this id</response>
        [HttpPost("{id}/topics")]
        [AuthenticationRequired]
        [ProducesResponseType(typeof(Envelope<Topic>), 201)]
        [ProducesResponseType(typeof(BadRequestError), 400)]
        [ProducesResponseType(typeof(GeneralError), 401)]
        [ProducesResponseType(typeof(GeneralError), 403)]
        [ProducesResponseType(typeof(GeneralError), 404)]
        public Task<Envelope<Topic>> PostTopic(string id, [FromBody] Topic topic) =>
            topicApiService.Create(id, topic);
    }
}