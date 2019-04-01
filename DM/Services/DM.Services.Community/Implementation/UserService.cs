using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Services.Community.Dto;
using DM.Services.Community.Repositories;
using DM.Services.Core.Dto;
using DM.Services.Core.Exceptions;

namespace DM.Services.Community.Implementation
{
    /// <inheritdoc />
    public class UserService : IUserService
    {
        private readonly IIdentity identity;
        private readonly IUserRepository repository;

        /// <inheritdoc />
        public UserService(
            IIdentity identity,
            IUserRepository repository)
        {
            this.identity = identity;
            this.repository = repository;
        }

        /// <inheritdoc />
        public async Task<(IEnumerable<GeneralUser> users, PagingData paging)> Get(int entityNumber, bool withInactive)
        {
            var totalUsersCount = await repository.CountUsers(withInactive);
            var paging = PagingData.Create(totalUsersCount, entityNumber, identity.Settings.TopicsPerPage);
            var users = await repository.GetUsers(paging, withInactive);
            return (users, paging);
        }

        /// <inheritdoc />
        public async Task<GeneralUser> Get(string login)
        {
            var user = await repository.GetUser(login);
            if (user == null)
            {
                throw new HttpException(HttpStatusCode.NotFound, $"User {login} not found");
            }

            return user;
        }

        /// <inheritdoc />
        public async Task<UserProfile> GetProfile(string login)
        {
            var profile = await repository.GetProfile(login);
            if (profile == null)
            {
                throw new HttpException(HttpStatusCode.NotFound, $"User {login} not found");
            }

            return profile;
        }
    }
}