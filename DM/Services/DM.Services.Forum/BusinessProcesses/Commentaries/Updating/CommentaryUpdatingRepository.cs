using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DM.Services.DataAccess;
using DM.Services.DataAccess.RelationalStorage;
using Microsoft.EntityFrameworkCore;
using Comment = DM.Services.DataAccess.BusinessObjects.Common.Comment;

namespace DM.Services.Forum.BusinessProcesses.Commentaries.Updating;

/// <inheritdoc />
internal class CommentaryUpdatingRepository : ICommentaryUpdatingRepository
{
    private readonly DmDbContext dbContext;
    private readonly IMapper mapper;

    /// <inheritdoc />
    public CommentaryUpdatingRepository(
        DmDbContext dbContext,
        IMapper mapper)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }
        
    /// <inheritdoc />
    public async Task<Services.Common.Dto.Comment> Update(IUpdateBuilder<Comment> update)
    {
        var commentId = update.AttachTo(dbContext);
        await dbContext.SaveChangesAsync();
        return await dbContext.Comments
            .TagWith("DM.Forum.UpdatedComment")
            .Where(c => c.CommentId == commentId)
            .ProjectTo<Services.Common.Dto.Comment>(mapper.ConfigurationProvider)
            .FirstAsync();
    }
}