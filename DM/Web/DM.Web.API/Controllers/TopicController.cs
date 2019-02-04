using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DM.Services.Forum.Implementation;
using DM.Web.API.Dto.Fora;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.API.Controllers
{
    [Route("v1")]
    public class TopicController : Controller
    {
        private readonly IForumService forumService;

        public TopicController(
            IForumService forumService)
        {
            this.forumService = forumService;
        }

        [HttpGet("fora/{id}/topics")]
        public async Task<IEnumerable<Topic>> Get(string id, [FromQuery] int entityNumber) =>
            (await forumService.GetTopicsList(id, entityNumber)).Topics.Select(t => new Topic(t));
    }
}