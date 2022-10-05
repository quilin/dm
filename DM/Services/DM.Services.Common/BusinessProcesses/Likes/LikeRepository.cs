using System;
using System.Threading.Tasks;
using DM.Services.DataAccess;
using DM.Services.DataAccess.BusinessObjects.Common;
using Microsoft.EntityFrameworkCore;

namespace DM.Services.Common.BusinessProcesses.Likes;

/// <inheritdoc />
internal class LikeRepository : ILikeRepository
{
    private readonly DmDbContext dbContext;

    /// <inheritdoc />
    public LikeRepository(
        DmDbContext dbContext)
    {
        this.dbContext = dbContext;
    }
        
    /// <inheritdoc />
    public Task Add(Like like)
    {
        dbContext.Likes.Add(like);
        return dbContext.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task Delete(Guid topicId, Guid userId)
    {
        var like = await dbContext.Likes.FirstAsync(l => l.UserId == userId && l.EntityId == topicId);
        dbContext.Likes.Remove(like);
        await dbContext.SaveChangesAsync();
    }
}