using System.Linq;
using System.Threading;
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
internal class PostUpdatingRepository(
    DmDbContext dbContext,
    IMapper mapper) : IPostUpdatingRepository
{
    /// <inheritdoc />
    public async Task<Post> Update(IUpdateBuilder<DbPost> updatePost, CancellationToken cancellationToken)
    {
        var postId = updatePost.AttachTo(dbContext);
        await dbContext.SaveChangesAsync(cancellationToken);
        return await dbContext.Posts
            .Where(p => p.PostId == postId)
            .ProjectTo<Post>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);
    }
}