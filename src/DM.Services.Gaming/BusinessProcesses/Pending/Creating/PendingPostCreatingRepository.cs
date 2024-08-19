using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DM.Services.DataAccess;
using DM.Services.Gaming.Dto.Output;
using Microsoft.EntityFrameworkCore;
using DbPendingPost = DM.Services.DataAccess.BusinessObjects.Games.Links.PendingPost;

namespace DM.Services.Gaming.BusinessProcesses.Pending.Creating;

/// <inheritdoc />
internal class PendingPostCreatingRepository(
    DmDbContext dbContext,
    IMapper mapper) : IPendingPostCreatingRepository
{
    /// <inheritdoc />
    public async Task<PendingPost> Create(DbPendingPost pendingPost, CancellationToken cancellationToken)
    {
        dbContext.PendingPosts.Add(pendingPost);
        await dbContext.SaveChangesAsync(cancellationToken);
        return await dbContext.PendingPosts
            .Where(p => p.PendingPostId == pendingPost.PendingPostId)
            .ProjectTo<PendingPost>(mapper.ConfigurationProvider)
            .FirstAsync(cancellationToken);
    }
}