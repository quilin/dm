using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DM.Services.Forum.BusinessProcesses.Fora;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Fora;

namespace DM.Web.API.Services.Fora;

/// <inheritdoc />
internal class ForumApiService(
    IForumReadingService forumService,
    IMapper mapper) : IForumApiService
{
    /// <param name="cancellationToken"></param>
    /// <inheritdoc />
    public async Task<ListEnvelope<Forum>> Get(CancellationToken cancellationToken)
    {
        var fora = await forumService.GetForaList(cancellationToken);
        return new ListEnvelope<Forum>(fora.Select(mapper.Map<Forum>));
    }

    /// <inheritdoc />
    public async Task<Envelope<Forum>> Get(string id, CancellationToken cancellationToken)
    {
        var forum = await forumService.GetSingleForum(id, cancellationToken);
        return new Envelope<Forum>(mapper.Map<Forum>(forum));
    }
}