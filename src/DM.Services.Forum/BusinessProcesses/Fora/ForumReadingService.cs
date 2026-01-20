using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
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
internal class ForumReadingService(
    IIdentityProvider identityProvider,
    IAccessPolicyConverter accessPolicyConverter,
    IForumRepository forumRepository,
    IUnreadCountersRepository unreadCountersRepository) : IForumReadingService
{
    /// <param name="cancellationToken"></param>
    /// <inheritdoc />
    public async Task<IEnumerable<Dto.Output.Forum>> GetForaList(CancellationToken cancellationToken)
    {
        var fora = await GetFora(true, cancellationToken);
        var identity = identityProvider.Current;
        if (identity.User.IsAuthenticated)
        {
            await unreadCountersRepository.FillParentCounters(fora, identity.User.UserId,
                f => f.Id, f => f.UnreadTopicsCount, cancellationToken);
        }

        return fora;
    }

    /// <inheritdoc />
    public async Task<Dto.Output.Forum> GetSingleForum(string forumTitle, CancellationToken cancellationToken)
    {
        var forum = await GetForum(forumTitle, true, cancellationToken);
        var identity = identityProvider.Current;
        if (identity.User.IsAuthenticated)
        {
            forum.UnreadTopicsCount = (await unreadCountersRepository.SelectByParents(
                identity.User.UserId, UnreadEntryType.Message, [forum.Id], cancellationToken))[forum.Id];
        }

        return forum;
    }

    /// <inheritdoc />
    public async Task<Dto.Output.Forum> GetForum(
        string forumTitle, bool onlyAvailable, CancellationToken cancellationToken)
    {
        var forum = (await GetFora(onlyAvailable, cancellationToken)).FirstOrDefault(f => f.Title == forumTitle);
        if (forum == null)
        {
            throw new HttpException(HttpStatusCode.Gone, $"Forum {forumTitle} not found");
        }

        return forum;
    }

    private async Task<Dto.Output.Forum[]> GetFora(bool onlyAvailable, CancellationToken cancellationToken)
    {
        var accessPolicy = onlyAvailable
            ? accessPolicyConverter.Convert(identityProvider.Current.User.Role)
            : (ForumAccessPolicy?) null;
        return (await forumRepository.SelectFora(accessPolicy, cancellationToken)).ToArray();
    }
}