using System;
using System.Threading;
using System.Threading.Tasks;
using DM.Services.DataAccess;
using DM.Services.DataAccess.BusinessObjects.Games.Links;
using Microsoft.EntityFrameworkCore;

namespace DM.Services.Gaming.BusinessProcesses.Readers.Subscribing;

/// <inheritdoc />
internal class ReadingSubscribingRepository(
    DmDbContext dbContext) : IReadingSubscribingRepository
{
    /// <inheritdoc />
    public Task<bool> HasSubscription(Guid userId, Guid gameId, CancellationToken cancellationToken) =>
        dbContext.Readers.AnyAsync(r => r.UserId == userId && r.GameId == gameId, cancellationToken);

    /// <inheritdoc />
    public Task Add(Reader reader, CancellationToken cancellationToken)
    {
        dbContext.Readers.Add(reader);
        return dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task Delete(Guid userId, Guid gameId, CancellationToken cancellationToken)
    {
        var reader = await dbContext.Readers
            .FirstAsync(r => r.GameId == gameId && r.UserId == userId, cancellationToken);
        dbContext.Readers.Remove(reader);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}