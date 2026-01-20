using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DM.Services.Common.Dto;
using DM.Services.Core.Dto;
using DM.Services.Core.Extensions;
using DM.Services.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace DM.Services.Gaming.BusinessProcesses.Commentaries.Reading;

/// <inheritdoc />
internal class CommentaryReadingRepository(
    DmDbContext dbContext,
    IMapper mapper) : ICommentaryReadingRepository
{
    /// <inheritdoc />
    public Task<int> Count(Guid gameId, CancellationToken cancellationToken) => dbContext.Comments
        .CountAsync(c => !c.IsRemoved && c.EntityId == gameId, cancellationToken);

    /// <inheritdoc />
    public async Task<IEnumerable<Comment>> Get(Guid gameId, PagingData paging, CancellationToken cancellationToken) =>
        await dbContext.Comments
            .Where(c => !c.IsRemoved && c.EntityId == gameId)
            .OrderBy(c => c.CreateDate)
            .Page(paging)
            .ProjectTo<Comment>(mapper.ConfigurationProvider)
            .ToArrayAsync(cancellationToken);

    /// <inheritdoc />
    public Task<Comment> Get(Guid commentId, CancellationToken cancellationToken) =>
        dbContext.Comments
            .Where(c => !c.IsRemoved && c.CommentId == commentId)
            .ProjectTo<Comment>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);
}