using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Services.Core.Dto;
using DM.Services.Forum.BusinessProcesses.Fora;

namespace DM.Services.Forum.BusinessProcesses.Moderation;

/// <inheritdoc />
internal class ModeratorsReadingService : IModeratorsReadingService
{
    private readonly IForumReadingService forumReadingService;
    private readonly IModeratorRepository moderatorRepository;

    /// <inheritdoc />
    public ModeratorsReadingService(
        IForumReadingService forumReadingService,
        IModeratorRepository moderatorRepository)
    {
        this.forumReadingService = forumReadingService;
        this.moderatorRepository = moderatorRepository;
    }
        
    /// <inheritdoc />
    public async Task<IEnumerable<GeneralUser>> GetModerators(string forumTitle)
    {
        var forum = await forumReadingService.GetForum(forumTitle);
        return await moderatorRepository.Get(forum.Id);
    }
}