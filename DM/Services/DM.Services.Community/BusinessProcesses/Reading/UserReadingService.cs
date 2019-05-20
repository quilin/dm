using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Services.Community.Dto;
using DM.Services.Core.Dto;
using DM.Services.Core.Exceptions;

namespace DM.Services.Community.BusinessProcesses.Reading
{
    /// <inheritdoc />
    public class UserReadingService : IUserReadingService
    {
        private readonly IIdentity identity;
        private readonly IUserReadingRepository readingRepository;

        /// <inheritdoc />
        public UserReadingService(
            IIdentity identity,
            IUserReadingRepository readingRepository)
        {
            this.identity = identity;
            this.readingRepository = readingRepository;
        }

        /// <inheritdoc />
        public async Task<(IEnumerable<GeneralUser> users, PagingResult paging)> Get(
            PagingQuery query, bool withInactive)
        {
            var totalCount = await readingRepository.CountUsers(withInactive);
            var paging = new PagingData(query, identity.Settings.TopicsPerPage, totalCount);
            var users = await readingRepository.GetUsers(paging, withInactive);
            return (users, paging.Result);
        }

        /// <inheritdoc />
        public async Task<GeneralUser> Get(string login)
        {
            var user = await readingRepository.GetUser(login);
            if (user == null)
            {
                throw new HttpException(HttpStatusCode.NotFound, $"User {login} not found");
            }

            return user;
        }

        /// <inheritdoc />
        public async Task<UserProfile> GetProfile(string login)
        {
            var profile = await readingRepository.GetProfile(login);
            if (profile == null)
            {
                throw new HttpException(HttpStatusCode.NotFound, $"User {login} not found");
            }

            return profile;
        }
    }
}