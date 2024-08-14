using System;
using System.Threading.Tasks;
using DM.Services.DataAccess;
using DM.Services.DataAccess.BusinessObjects.Games.Links;
using Microsoft.EntityFrameworkCore;

namespace DM.Services.Gaming.BusinessProcesses.Readers.Subscribing;

/// <inheritdoc />
internal class ReadingSubscribingRepository : IReadingSubscribingRepository
{
    private readonly DmDbContext dbContext;

    /// <inheritdoc />
    public ReadingSubscribingRepository(
        DmDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    /// <inheritdoc />
    public Task<bool> HasSubscription(Guid userId, Guid gameId) =>
        dbContext.Readers.AnyAsync(r => r.UserId == userId && r.GameId == gameId);

    /// <inheritdoc />
    public Task Add(Reader reader)
    {
        dbContext.Readers.Add(reader);
        return dbContext.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task Delete(Guid userId, Guid gameId)
    {
        var reader = await dbContext.Readers.FirstAsync(r => r.GameId == gameId && r.UserId == userId);
        dbContext.Readers.Remove(reader);
        await dbContext.SaveChangesAsync();
    }
}