using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Common.Extensions;
using DM.Services.Common.Repositories;
using DM.Services.Core.Dto.Enums;
using DM.Services.Core.Exceptions;
using DM.Services.Forum.BusinessProcesses.Common;
using Microsoft.Extensions.Caching.Memory;

namespace DM.Services.Forum.BusinessProcesses.Fora
{
    /// <inheritdoc />
    public class ForumReadingService : IForumReadingService
    {
        private readonly IIdentity identity;
        private readonly IAccessPolicyConverter accessPolicyConverter;
        private readonly IMemoryCache memoryCache;
        private readonly IForumRepository forumRepository;
        private readonly IUnreadCountersRepository unreadCountersRepository;

        /// <inheritdoc />
        public ForumReadingService(
            IIdentityProvider identityProvider,
            IAccessPolicyConverter accessPolicyConverter,
            IMemoryCache memoryCache,
            IForumRepository forumRepository,
            IUnreadCountersRepository unreadCountersRepository)
        {
            identity = identityProvider.Current;
            this.accessPolicyConverter = accessPolicyConverter;
            this.memoryCache = memoryCache;
            this.forumRepository = forumRepository;
            this.unreadCountersRepository = unreadCountersRepository;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Dto.Forum>> GetForaList()
        {
            var fora = await GetFora();
            await unreadCountersRepository.FillParentCounters(fora, identity.User.UserId,
                f => f.Id, f => f.UnreadTopicsCount);
            return fora;
        }

        /// <inheritdoc />
        public async Task<Dto.Forum> GetForumWithCounters(string forumTitle)
        {
            var forum = await GetForum(forumTitle);
            await unreadCountersRepository.FillParentCounters(new[] {forum}, identity.User.UserId,
                f => f.Id, f => f.UnreadTopicsCount);
            return forum;
        }

        /// <inheritdoc />
        public async Task<Dto.Forum> GetForum(string forumTitle, bool onlyAvailable = true)
        {
            var forum = (await GetFora(onlyAvailable)).FirstOrDefault(f => f.Title == forumTitle);
            if (forum == null)
            {
                throw new HttpException(HttpStatusCode.NotFound, $"Forum {forumTitle} not found");
            }

            return forum;
        }

        private async Task<Dto.Forum[]> GetFora(bool onlyAvailable = true)
        {
            var accessPolicy = onlyAvailable
                ? accessPolicyConverter.Convert(identity.User.Role)
                : (ForumAccessPolicy?) null;
            return await memoryCache.GetOrCreateAsync($"ForaList_{accessPolicy}", async _ =>
                (await forumRepository.SelectFora(accessPolicy)).ToArray());
        }
    }
}