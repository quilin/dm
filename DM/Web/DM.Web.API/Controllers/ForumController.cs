using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Services.Forum.Dto;
using DM.Services.Forum.Implementation;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.API.Controllers
{
    [Route("v1/fora")]
    public class ForumController : Controller
    {
        private readonly IForumService forumService;

        public ForumController(
            IForumService forumService)
        {
            this.forumService = forumService;
        }

        [HttpGet("")]
        public Task<IEnumerable<ForaListItem>> Get() => forumService.GetForaList();

        [HttpGet("{id}/topics")]
        public async Task<IEnumerable<TopicsListItem>> Get(string id, [FromQuery] int n) =>
            (await forumService.GetTopicsList(id, n)).Topics;
    }
}