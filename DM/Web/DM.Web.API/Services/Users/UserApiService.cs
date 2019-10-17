using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DM.Services.Community.BusinessProcesses.Reading;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Users;

namespace DM.Web.API.Services.Users
{
    /// <inheritdoc />
    public class UserApiService : IUserApiService
    {
        private readonly IUserReadingService userReadingService;
        private readonly IMapper mapper;

        /// <inheritdoc />
        public UserApiService(
            IUserReadingService userReadingService,
            IMapper mapper)
        {
            this.userReadingService = userReadingService;
            this.mapper = mapper;
        }

        /// <inheritdoc />
        public async Task<ListEnvelope<User>> GetUsers(UsersQuery query)
        {
            var (users, paging) = await userReadingService.Get(query, query.Inactive);
            return new ListEnvelope<User>(users.Select(mapper.Map<User>), new Paging(paging));
        }

        /// <inheritdoc />
        public async Task<Envelope<User>> GetUser(string login)
        {
            var user = await userReadingService.Get(login);
            return new Envelope<User>(mapper.Map<User>(user));
        }
    }
}