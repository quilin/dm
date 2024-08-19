using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DM.Services.DataAccess;
using DM.Services.DataAccess.BusinessObjects.Fora;
using DM.Services.DataAccess.RelationalStorage;
using Microsoft.EntityFrameworkCore;
using Comment = DM.Services.DataAccess.BusinessObjects.Common.Comment;

namespace DM.Services.Forum.BusinessProcesses.Commentaries.Creating;

/// <inheritdoc />
internal class CommentaryCreatingRepository(
    DmDbContext dbContext,
    IMapper mapper) : ICommentaryCreatingRepository
{
    /// <inheritdoc />
    public async Task<Services.Common.Dto.Comment> Create(Comment comment, IUpdateBuilder<ForumTopic> topicUpdate,
        CancellationToken cancellationToken)
    {
        dbContext.Comments.Add(comment);
        topicUpdate.AttachTo(dbContext);
        await dbContext.SaveChangesAsync();
        return await dbContext.Comments
            .TagWith("DM.Forum.CreatedComment")
            .Where(c => c.CommentId == comment.CommentId)
            .ProjectTo<Services.Common.Dto.Comment>(mapper.ConfigurationProvider)
            .FirstAsync();
    }
}