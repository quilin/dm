using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DM.Services.Forum.Implementation;
using DM.Web.API.Dto.Fora;
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
        public async Task<IEnumerable<Forum>> Get() => (await forumService.GetForaList()).Select(f => new Forum(f));

        [HttpGet("{id}")]
        public async Task<Forum> Get(string id) => new Forum(await forumService.GetForum(id));
    }
}