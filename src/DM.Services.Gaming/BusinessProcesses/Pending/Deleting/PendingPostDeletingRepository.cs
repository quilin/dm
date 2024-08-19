using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DM.Services.DataAccess;
using DM.Services.DataAccess.RelationalStorage;
using DM.Services.Gaming.Dto.Output;
using Microsoft.EntityFrameworkCore;
using DbPendingPost = DM.Services.DataAccess.BusinessObjects.Games.Links.PendingPost;

namespace DM.Services.Gaming.BusinessProcesses.Pending.Deleting;

/// <inheritdoc />
internal class PendingPostDeletingRepository(
    DmDbContext dbContext,
    IMapper mapper) : IPendingPostDeletingRepository
{
    /// <inheritdoc />
    public Task<PendingPost> Get(Guid pendingPostId, CancellationToken cancellationToken) =>
        dbContext.PendingPosts
            .Where(p => p.PendingPostId == pendingPostId)
            .ProjectTo<PendingPost>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

    /// <inheritdoc />
    public Task Delete(IUpdateBuilder<DbPendingPost> updateBuilder, CancellationToken cancellationToken)
    {
        updateBuilder.AttachTo(dbContext);
        return dbContext.SaveChangesAsync(cancellationToken);
    }
}