using AutoMapper;
using AutoMapper.QueryableExtensions;
using DM.Services.Common.Dto;
using DM.Services.Core.Dto;
using DM.Services.Core.Extensions;
using DM.Services.DataAccess;
using DM.Services.Forum.BusinessProcesses.Commentaries.Reading;
using Microsoft.EntityFrameworkCore;

namespace DM.Services.Forum.Storage.Storages.Commentaries;

/// <inheritdoc />
internal class CommentaryReadingRepository(
    DmDbContext dbContext,
    IMapper mapper) : ICommentaryReadingRepository
{
    /// <inheritdoc />
    public Task<int> Count(Guid topicId, CancellationToken cancellationToken) =>
        dbContext.Comments
            .TagWith("DM.Forum.CommentsCount")
            .CountAsync(c => !c.IsRemoved && c.EntityId == topicId, cancellationToken);

    /// <inheritdoc />
    public async Task<IEnumerable<Comment>> Get(
        Guid topicId, PagingData paging, CancellationToken cancellationToken) =>
        await dbContext.Comments
            .TagWith("DM.Forum.CommentsList")
            .Where(c => !c.IsRemoved && c.EntityId == topicId)
            .OrderBy(c => c.CreateDate)
            .Page(paging)
            .ProjectTo<Comment>(mapper.ConfigurationProvider)
            .ToArrayAsync(cancellationToken);

    /// <inheritdoc />
    public Task<Comment?> Get(Guid commentId, CancellationToken cancellationToken) =>
        dbContext.Comments
            .TagWith("DM.Forum.Comment")
            .Where(c => !c.IsRemoved && c.CommentId == commentId)
            .ProjectTo<Comment>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);
}