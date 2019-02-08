using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Fora;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.API.Controllers.Fora
{
    [Route("v1/fora")]
    public class ForumController : Controller
    {
        [HttpGet("")]
        [ProducesResponseType(typeof(ListEnvelope<Forum>), 200)]
        public ListEnvelope<Forum> GetFora() => null;

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Envelope<Forum>), 200)]
        [ProducesResponseType(typeof(GeneralError), 404)]
        public Envelope<Forum> GetForum(string id) => null;

        [HttpGet("{id}/topics")]
        [ProducesResponseType(typeof(ListEnvelope<Topic>), 200)]
        [ProducesResponseType(typeof(BadRequestError), 400)]
        [ProducesResponseType(typeof(GeneralError), 404)]
        public ListEnvelope<Topic> GetTopics(string id, [FromQuery] TopicFilters f, [FromQuery] int n = 1) => null;

        [HttpPost("{id}/topics")]
        [ProducesResponseType(typeof(Envelope<Topic>), 201)]
        [ProducesResponseType(typeof(BadRequestError), 400)]
        [ProducesResponseType(typeof(GeneralError), 403)]
        [ProducesResponseType(typeof(GeneralError), 404)]
        public Envelope<Topic> PostTopic(string id, [FromBody] Topic topic) => null;
    }
}