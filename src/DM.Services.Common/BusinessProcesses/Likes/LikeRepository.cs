using System;
using System.Threading;
using System.Threading.Tasks;
using DM.Services.DataAccess;
using DM.Services.DataAccess.BusinessObjects.Common;
using Microsoft.EntityFrameworkCore;

namespace DM.Services.Common.BusinessProcesses.Likes;

/// <inheritdoc />
internal class LikeRepository(
    DmDbContext dbContext) : ILikeRepository
{
    /// <inheritdoc />
    public Task Add(Like like, CancellationToken cancellationToken)
    {
        dbContext.Likes.Add(like);
        return dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task Delete(Guid topicId, Guid userId, CancellationToken cancellationToken)
    {
        var like = await dbContext.Likes
            .FirstAsync(l => l.UserId == userId && l.EntityId == topicId, cancellationToken);
        dbContext.Likes.Remove(like);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}