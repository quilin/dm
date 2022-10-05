using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Common.BusinessProcesses.UnreadCounters;
using DM.Services.Common.Extensions;
using DM.Services.Core.Dto.Enums;
using DM.Services.Core.Exceptions;
using DM.Services.DataAccess.BusinessObjects.Common;
using DM.Services.Forum.BusinessProcesses.Common;

namespace DM.Services.Forum.BusinessProcesses.Fora;

/// <inheritdoc />
internal class ForumReadingService : IForumReadingService
{
    private readonly IIdentityProvider identityProvider;
    private readonly IAccessPolicyConverter accessPolicyConverter;
    private readonly IForumRepository forumRepository;
    private readonly IUnreadCountersRepository unreadCountersRepository;

    /// <inheritdoc />
    public ForumReadingService(
        IIdentityProvider identityProvider,
        IAccessPolicyConverter accessPolicyConverter,
        IForumRepository forumRepository,
        IUnreadCountersRepository unreadCountersRepository)
    {
        this.identityProvider = identityProvider;
        this.accessPolicyConverter = accessPolicyConverter;
        this.forumRepository = forumRepository;
        this.unreadCountersRepository = unreadCountersRepository;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Dto.Output.Forum>> GetForaList()
    {
        var fora = await GetFora();
        var identity = identityProvider.Current;
        if (identity.User.IsAuthenticated)
        {
            await unreadCountersRepository.FillParentCounters(fora, identity.User.UserId,
                f => f.Id, f => f.UnreadTopicsCount);
        }

        return fora;
    }

    /// <inheritdoc />
    public async Task<Dto.Output.Forum> GetSingleForum(string forumTitle)
    {
        var forum = await GetForum(forumTitle);
        var identity = identityProvider.Current;
        if (identity.User.IsAuthenticated)
        {
            forum.UnreadTopicsCount = (await unreadCountersRepository.SelectByParents(
                identity.User.UserId, UnreadEntryType.Message, forum.Id))[forum.Id];
        }

        return forum;
    }

    /// <inheritdoc />
    public async Task<Dto.Output.Forum> GetForum(string forumTitle, bool onlyAvailable = true)
    {
        var forum = (await GetFora(onlyAvailable)).FirstOrDefault(f => f.Title == forumTitle);
        if (forum == null)
        {
            throw new HttpException(HttpStatusCode.Gone, $"Forum {forumTitle} not found");
        }

        return forum;
    }

    private async Task<Dto.Output.Forum[]> GetFora(bool onlyAvailable = true)
    {
        var accessPolicy = onlyAvailable
            ? accessPolicyConverter.Convert(identityProvider.Current.User.Role)
            : (ForumAccessPolicy?) null;
        return (await forumRepository.SelectFora(accessPolicy)).ToArray();
    }
}