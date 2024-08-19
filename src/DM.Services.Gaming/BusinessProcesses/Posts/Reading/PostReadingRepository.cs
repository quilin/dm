using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DM.Services.Core.Dto;
using DM.Services.Core.Extensions;
using DM.Services.DataAccess;
using DM.Services.Gaming.BusinessProcesses.Shared;
using DM.Services.Gaming.Dto.Output;
using Microsoft.EntityFrameworkCore;

namespace DM.Services.Gaming.BusinessProcesses.Posts.Reading;

/// <inheritdoc />
internal class PostReadingRepository(
    DmDbContext dbContext,
    IMapper mapper) : IPostReadingRepository
{
    /// <inheritdoc />
    public Task<int> Count(Guid roomId, Guid userId, CancellationToken cancellationToken) =>
        dbContext.Rooms
            .Where(AccessibilityFilters.RoomAvailable(userId))
            .Where(r => r.RoomId == roomId)
            .SelectMany(r => r.Posts)
            .Where(p => !p.IsRemoved)
            .CountAsync(cancellationToken);

    /// <inheritdoc />
    public async Task<IEnumerable<Post>> Get(Guid roomId, PagingData paging, Guid userId,
        CancellationToken cancellationToken) =>
        await dbContext.Rooms
            .Where(AccessibilityFilters.RoomAvailable(userId))
            .Where(r => r.RoomId == roomId)
            .SelectMany(r => r.Posts)
            .Where(p => !p.IsRemoved)
            .OrderBy(p => p.CreateDate)
            .Page(paging)
            .ProjectTo<Post>(mapper.ConfigurationProvider)
            .ToArrayAsync(cancellationToken);

    /// <inheritdoc />
    public Task<Post> Get(Guid postId, Guid userId, CancellationToken cancellationToken) =>
        dbContext.Rooms
            .Where(AccessibilityFilters.RoomAvailable(userId))
            .SelectMany(r => r.Posts)
            .Where(p => !p.IsRemoved && p.PostId == postId)
            .ProjectTo<Post>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);
}