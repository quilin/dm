using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DM.Services.Forum.Implementation;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Users;

namespace DM.Web.API.Services.Fora
{
    public class ModeratorsApiService : IModeratorsApiService
    {
        private readonly IForumService forumService;
        private readonly IMapper mapper;

        public ModeratorsApiService(
            IForumService forumService,
            IMapper mapper)
        {
            this.forumService = forumService;
            this.mapper = mapper;
        }
        
        public async Task<ListEnvelope<User>> GetModerators(string id)
        {
            var moderators = await forumService.GetModerators(id);
            return new ListEnvelope<User>(moderators.Select(mapper.Map<User>));
        }
    }
}