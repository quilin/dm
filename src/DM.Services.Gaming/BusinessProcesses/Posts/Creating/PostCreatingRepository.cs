using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DM.Services.DataAccess;
using DM.Services.DataAccess.RelationalStorage;
using DM.Services.Gaming.Dto.Output;
using Microsoft.EntityFrameworkCore;
using PendingPost = DM.Services.DataAccess.BusinessObjects.Games.Links.PendingPost;
using DbPost = DM.Services.DataAccess.BusinessObjects.Games.Posts.Post;

namespace DM.Services.Gaming.BusinessProcesses.Posts.Creating;

/// <inheritdoc />
internal class PostCreatingRepository : IPostCreatingRepository
{
    private readonly DmDbContext dbContext;
    private readonly IMapper mapper;

    /// <inheritdoc />
    public PostCreatingRepository(
        DmDbContext dbContext,
        IMapper mapper)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<Post> Create(DbPost post, IEnumerable<IUpdateBuilder<PendingPost>> pendingPostUpdates)
    {
        dbContext.Posts.Add(post);
        foreach (var pendingPostUpdate in pendingPostUpdates)
        {
            pendingPostUpdate.AttachTo(dbContext);
        }

        await dbContext.SaveChangesAsync();
        return await dbContext.Posts
            .Where(p => p.PostId == post.PostId)
            .ProjectTo<Post>(mapper.ConfigurationProvider)
            .FirstAsync();
    }
}