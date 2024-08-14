using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DM.Services.DataAccess;
using DM.Services.Gaming.Dto.Output;
using Microsoft.EntityFrameworkCore;

namespace DM.Services.Gaming.BusinessProcesses.Pending.Creating;

/// <inheritdoc />
internal class PendingPostCreatingRepository : IPendingPostCreatingRepository
{
    private readonly DmDbContext dbContext;
    private readonly IMapper mapper;

    /// <inheritdoc />
    public PendingPostCreatingRepository(
        DmDbContext dbContext,
        IMapper mapper)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<PendingPost> Create(DataAccess.BusinessObjects.Games.Links.PendingPost pendingPost)
    {
        dbContext.PendingPosts.Add(pendingPost);
        await dbContext.SaveChangesAsync();
        return await dbContext.PendingPosts
            .Where(p => p.PendingPostId == pendingPost.PendingPostId)
            .ProjectTo<PendingPost>(mapper.ConfigurationProvider)
            .FirstAsync();
    }
}