using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DM.Services.Core.Dto;
using DM.Services.Forum.BusinessProcesses.Fora;

namespace DM.Services.Forum.BusinessProcesses.Moderation;

/// <inheritdoc />
internal class ModeratorsReadingService(
    IForumReadingService forumReadingService,
    IModeratorRepository moderatorRepository) : IModeratorsReadingService
{
    /// <inheritdoc />
    public async Task<IEnumerable<GeneralUser>> GetModerators(string forumTitle, CancellationToken cancellationToken)
    {
        var forum = await forumReadingService.GetForum(forumTitle, true, cancellationToken);
        return await moderatorRepository.Get(forum.Id, cancellationToken);
    }
}