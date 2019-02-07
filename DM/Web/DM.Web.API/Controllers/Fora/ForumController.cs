using DM.Web.API.Dto.Common;
using DM.Web.API.Dto.Fora;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.API.Controllers.Fora
{
    [Route("v1/fora")]
    public class ForumController : Controller
    {
        [HttpGet("")]
        public ListEnvelope<Forum> GetFora() => null;

        [HttpGet("{id}")]
        public Envelope<Forum> GetForum(string id) => null;

        [HttpGet("{id}/topics")]
        public ListEnvelope<Topic> GetTopics(string id, [FromQuery] TopicFilters f, [FromQuery] int n = 1) => null;

        [HttpPost("{id}/topics")]
        public Envelope<Topic> PostTopic(string id, [FromBody] Topic topic) => null;
    }
}