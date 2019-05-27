using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Community.Dto;
using DM.Services.Core.Dto;
using DM.Services.Core.Exceptions;

namespace DM.Services.Community.BusinessProcesses.Reading
{
    /// <inheritdoc />
    public class UserReadingService : IUserReadingService
    {
        private readonly IIdentityProvider identityProvider;
        private readonly IUserReadingRepository readingRepository;

        /// <inheritdoc />
        public UserReadingService(
            IIdentityProvider identityProvider,
            IUserReadingRepository readingRepository)
        {
            this.identityProvider = identityProvider;
            this.readingRepository = readingRepository;
        }

        /// <inheritdoc />
        public async Task<(IEnumerable<GeneralUser> users, PagingResult paging)> Get(
            PagingQuery query, bool withInactive)
        {
            var totalCount = await readingRepository.CountUsers(withInactive);
            var paging = new PagingData(query, identityProvider.Current.Settings.TopicsPerPage, totalCount);
            var users = await readingRepository.GetUsers(paging, withInactive);
            return (users, paging.Result);
        }

        /// <inheritdoc />
        public async Task<GeneralUser> Get(string login)
        {
            var user = await readingRepository.GetUser(login);
            if (user == null)
            {
                throw new HttpException(HttpStatusCode.Gone, $"User {login} not found");
            }

            return user;
        }

        /// <inheritdoc />
        public async Task<UserProfile> GetProfile(string login)
        {
            var profile = await readingRepository.GetProfile(login);
            if (profile == null)
            {
                throw new HttpException(HttpStatusCode.Gone, $"User {login} not found");
            }

            return profile;
        }
    }
}