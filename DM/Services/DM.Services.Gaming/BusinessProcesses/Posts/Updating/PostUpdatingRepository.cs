using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DM.Services.DataAccess;
using DM.Services.DataAccess.RelationalStorage;
using DM.Services.Gaming.Dto.Output;
using Microsoft.EntityFrameworkCore;
using DbPost = DM.Services.DataAccess.BusinessObjects.Games.Posts.Post;

namespace DM.Services.Gaming.BusinessProcesses.Posts.Updating;

/// <inheritdoc />
internal class PostUpdatingRepository : IPostUpdatingRepository
{
    private readonly DmDbContext dbContext;
    private readonly IMapper mapper;

    /// <inheritdoc />
    public PostUpdatingRepository(
        DmDbContext dbContext,
        IMapper mapper)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<Post> Update(IUpdateBuilder<DbPost> updatePost)
    {
        var postId = updatePost.AttachTo(dbContext);
        await dbContext.SaveChangesAsync();
        return await dbContext.Posts
            .Where(p => p.PostId == postId)
            .ProjectTo<Post>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();
    }
}