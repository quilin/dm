using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DM.Services.Community.BusinessProcesses.Users.Reading;
using DM.Services.Community.BusinessProcesses.Users.Updating;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Users;

namespace DM.Web.API.Services.Users
{
    /// <inheritdoc />
    public class UserApiService : IUserApiService
    {
        private readonly IUserReadingService readingService;
        private readonly IUserUpdatingService updatingService;
        private readonly IMapper mapper;

        /// <inheritdoc />
        public UserApiService(
            IUserReadingService readingService,
            IUserUpdatingService updatingService,
            IMapper mapper)
        {
            this.readingService = readingService;
            this.updatingService = updatingService;
            this.mapper = mapper;
        }

        /// <inheritdoc />
        public async Task<ListEnvelope<User>> GetUsers(UsersQuery query)
        {
            var (users, paging) = await readingService.Get(query, query.Inactive);
            return new ListEnvelope<User>(users.Select(mapper.Map<User>), new Paging(paging));
        }

        /// <inheritdoc />
        public async Task<Envelope<User>> GetUser(string login)
        {
            var user = await readingService.GetDetails(login);
            return new Envelope<User>(mapper.Map<User>(user));
        }

        /// <inheritdoc />
        public async Task<Envelope<User>> UpdateUser(string login, User user)
        {
            var updateUser = mapper.Map<UpdateUser>(user);
            updateUser.Login = login;
            var updatedUser = await updatingService.Update(updateUser);
            return new Envelope<User>(mapper.Map<User>(updatedUser));
        }
    }
}