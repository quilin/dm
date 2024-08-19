using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DM.Services.Common.Dto;
using DM.Services.DataAccess;
using Microsoft.EntityFrameworkCore;
using DbComment = DM.Services.DataAccess.BusinessObjects.Common.Comment;

namespace DM.Services.Gaming.BusinessProcesses.Commentaries.Creating;

/// <inheritdoc />
internal class CommentaryCreatingRepository(
    DmDbContext dbContext,
    IMapper mapper) : ICommentaryCreatingRepository
{
    /// <inheritdoc />
    public async Task<Comment> Create(DbComment comment, CancellationToken cancellationToken)
    {
        dbContext.Comments.Add(comment);
        await dbContext.SaveChangesAsync(cancellationToken);
        return await dbContext.Comments
            .Where(c => c.CommentId == comment.CommentId)
            .ProjectTo<Comment>(mapper.ConfigurationProvider)
            .FirstAsync(cancellationToken);
    }
}