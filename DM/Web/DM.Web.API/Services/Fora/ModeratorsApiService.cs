using System.Linq;
using System.Threading.Tasks;
using DM.Services.Forum.Implementation;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Users;

namespace DM.Web.API.Services.Fora
{
    public class ModeratorsApiService : IModeratorsApiService
    {
        private readonly IForumService forumService;

        public ModeratorsApiService(
            IForumService forumService)
        {
            this.forumService = forumService;
        }
        
        public async Task<ListEnvelope<User>> GetModerators(string id)
        {
            var moderators = await forumService.GetModerators(id);
            return new ListEnvelope<User>(moderators.Select(m => new User(m)));
        }
    }
}