using System;
using System.Linq;
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
internal class PendingPostDeletingRepository : IPendingPostDeletingRepository
{
    private readonly DmDbContext dbContext;
    private readonly IMapper mapper;

    /// <inheritdoc />
    public PendingPostDeletingRepository(
        DmDbContext dbContext,
        IMapper mapper)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }
        
    /// <inheritdoc />
    public Task<PendingPost> Get(Guid pendingPostId)
    {
        return dbContext.PendingPosts
            .Where(p => p.PendingPostId == pendingPostId)
            .ProjectTo<PendingPost>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();
    }

    /// <inheritdoc />
    public Task Delete(IUpdateBuilder<DbPendingPost> updateBuilder)
    {
        updateBuilder.AttachTo(dbContext);
        return dbContext.SaveChangesAsync();
    }
}