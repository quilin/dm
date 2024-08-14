using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DM.Services.Common.Dto;
using DM.Services.DataAccess;
using Microsoft.EntityFrameworkCore;
using DbComment = DM.Services.DataAccess.BusinessObjects.Common.Comment;

namespace DM.Services.Gaming.BusinessProcesses.Commentaries.Creating;

/// <inheritdoc />
internal class CommentaryCreatingRepository : ICommentaryCreatingRepository
{
    private readonly DmDbContext dbContext;
    private readonly IMapper mapper;

    /// <inheritdoc />
    public CommentaryCreatingRepository(
        DmDbContext dbContext,
        IMapper mapper)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<Comment> Create(DbComment comment)
    {
        dbContext.Comments.Add(comment);
        await dbContext.SaveChangesAsync();
        return await dbContext.Comments
            .Where(c => c.CommentId == comment.CommentId)
            .ProjectTo<Comment>(mapper.ConfigurationProvider)
            .FirstAsync();
    }
}