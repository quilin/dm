using System.Linq;
using System.Threading.Tasks;
using DM.Services.Forum.Implementation;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Fora;

namespace DM.Web.API.Services.Fora
{
    public class ForumApiService : IForumApiService
    {
        private readonly IForumService forumService;

        public ForumApiService(
            IForumService forumService)
        {
            this.forumService = forumService;
        }
        
        public async Task<ListEnvelope<Forum>> Get()
        {
            var fora = await forumService.GetForaList();
            return new ListEnvelope<Forum>(fora.Select(f => new Forum(f)));
        }

        public async Task<Envelope<Forum>> Get(string id)
        {
            var forum = await forumService.GetForum(id);
            return new Envelope<Forum>(new Forum(forum));
        }
    }
}