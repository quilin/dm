using AutoMapper;
using AutoMapper.QueryableExtensions;
using DM.Services.DataAccess;
using DM.Services.DataAccess.BusinessObjects.Common;
using DM.Services.DataAccess.BusinessObjects.Fora;
using DM.Services.DataAccess.RelationalStorage;
using DM.Services.Forum.BusinessProcesses.Commentaries.Deleting;
using DM.Services.Forum.Dto.Internal;
using Microsoft.EntityFrameworkCore;

namespace DM.Services.Forum.Storage.Storages.Commentaries;

/// <inheritdoc />
internal class CommentaryDeletingRepository(
    DmDbContext dbContext,
    IMapper mapper) : ICommentaryDeletingRepository
{
    /// <inheritdoc />
    public Task<CommentToDelete?> GetForDelete(Guid commentId, CancellationToken cancellationToken) =>
        dbContext.Comments
            .TagWith("DM.Forum.CommentToDelete")
            .Where(c => !c.IsRemoved && c.CommentId == commentId)
            .ProjectTo<CommentToDelete>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

    /// <inheritdoc />
    public Task Delete(IUpdateBuilder<Comment> update, IUpdateBuilder<ForumTopic> topicUpdate,
        CancellationToken cancellationToken)
    {
        update.AttachTo(dbContext);
        topicUpdate.AttachTo(dbContext);
        return dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<Guid?> GetSecondLastCommentId(Guid topicId, CancellationToken cancellationToken)
    {
        var result = await dbContext.Comments
            .TagWith("DM.Forum.SecondLastCommentAfterDelete")
            .Where(c => !c.IsRemoved && c.EntityId == topicId)
            .OrderByDescending(c => c.CreateDate)
            .Skip(1)
            .Select(c => c.CommentId)
            .FirstOrDefaultAsync(cancellationToken);
        return result == default ? null : result;
    }
}