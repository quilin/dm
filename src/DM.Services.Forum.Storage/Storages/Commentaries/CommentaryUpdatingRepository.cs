using AutoMapper;
using AutoMapper.QueryableExtensions;
using DM.Services.DataAccess;
using DM.Services.DataAccess.RelationalStorage;
using DM.Services.Forum.BusinessProcesses.Commentaries.Updating;
using Microsoft.EntityFrameworkCore;
using Comment = DM.Services.DataAccess.BusinessObjects.Common.Comment;

namespace DM.Services.Forum.Storage.Storages.Commentaries;

/// <inheritdoc />
internal class CommentaryUpdatingRepository(
    DmDbContext dbContext,
    IMapper mapper) : ICommentaryUpdatingRepository
{
    /// <inheritdoc />
    public async Task<Services.Common.Dto.Comment> Update(
        IUpdateBuilder<Comment> update, CancellationToken cancellationToken)
    {
        var commentId = update.AttachTo(dbContext);
        await dbContext.SaveChangesAsync(cancellationToken);
        return await dbContext.Comments
            .TagWith("DM.Forum.UpdatedComment")
            .Where(c => c.CommentId == commentId)
            .ProjectTo<Services.Common.Dto.Comment>(mapper.ConfigurationProvider)
            .FirstAsync(cancellationToken);
    }
}