using System;
using System.Collections.Generic;
using System.Linq;
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
internal class CommentaryReadingRepository : ICommentaryReadingRepository
{
    private readonly DmDbContext dbContext;
    private readonly IMapper mapper;

    /// <inheritdoc />
    public CommentaryReadingRepository(
        DmDbContext dbContext,
        IMapper mapper)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }

    /// <inheritdoc />
    public Task<int> Count(Guid gameId) => dbContext.Comments
        .CountAsync(c => !c.IsRemoved && c.EntityId == gameId);

    /// <inheritdoc />
    public async Task<IEnumerable<Comment>> Get(Guid gameId, PagingData paging)
    {
        return await dbContext.Comments
            .Where(c => !c.IsRemoved && c.EntityId == gameId)
            .OrderBy(c => c.CreateDate)
            .Page(paging)
            .ProjectTo<Comment>(mapper.ConfigurationProvider)
            .ToArrayAsync();
    }

    /// <inheritdoc />
    public Task<Comment> Get(Guid commentId)
    {
        return dbContext.Comments
            .Where(c => !c.IsRemoved && c.CommentId == commentId)
            .ProjectTo<Comment>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();
    }
}