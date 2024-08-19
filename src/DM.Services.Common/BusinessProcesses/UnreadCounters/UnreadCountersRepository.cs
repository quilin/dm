using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DM.Services.Core.Implementation;
using DM.Services.DataAccess.BusinessObjects.Common;
using DM.Services.DataAccess.MongoIntegration;
using MongoDB.Driver;

namespace DM.Services.Common.BusinessProcesses.UnreadCounters;

/// <inheritdoc cref="IUnreadCountersRepository" />
internal class UnreadCountersRepository(
    DmMongoClient client,
    IDateTimeProvider dateTimeProvider) : MongoCollectionRepository<UnreadCounter>(client), IUnreadCountersRepository
{
    /// <inheritdoc />
    public Task Create(Guid entityId, UnreadEntryType entryType,
        IReadOnlyCollection<Guid> userIds, CancellationToken cancellationToken)
    {
        return Collection.InsertManyAsync(userIds.Select(id => new UnreadCounter
        {
            UserId = id,
            EntityId = entityId,
            ParentId = id,
            EntryType = entryType,
            LastRead = dateTimeProvider.Now.UtcDateTime,
            Counter = 0
        }), cancellationToken: cancellationToken);
    }

    /// <inheritdoc />
    public Task Create(Guid entityId, Guid parentId, UnreadEntryType entryType, CancellationToken cancellationToken)
    {
        return Collection.InsertOneAsync(new UnreadCounter
        {
            UserId = Guid.Empty,
            EntityId = entityId,
            ParentId = parentId,
            EntryType = entryType,
            LastRead = dateTimeProvider.Now.UtcDateTime,
            Counter = 0
        }, cancellationToken: cancellationToken);
    }

    /// <inheritdoc />
    public Task Create(Guid entityId, UnreadEntryType entryType, CancellationToken cancellationToken) =>
        Create(entityId, entityId, entryType, cancellationToken);

    /// <inheritdoc />
    public Task Increment(Guid entityId, UnreadEntryType entryType, CancellationToken cancellationToken)
    {
        return Collection.UpdateManyAsync(
            Filter.Eq(c => c.EntityId, entityId) &
            Filter.Eq(c => c.EntryType, entryType),
            Update.Inc(c => c.Counter, 1),
            cancellationToken: cancellationToken);
    }

    /// <inheritdoc />
    public Task Decrement(Guid entityId, UnreadEntryType entryType,
        DateTimeOffset createDate, CancellationToken cancellationToken)
    {
        return Collection.UpdateManyAsync(
            Filter.Eq(c => c.EntityId, entityId) &
            Filter.Eq(c => c.EntryType, entryType) &
            Filter.Lt(c => c.LastRead, createDate.UtcDateTime),
            Update.Inc(c => c.Counter, -1),
            cancellationToken: cancellationToken);
    }

    /// <inheritdoc />
    public Task Delete(Guid entityId, UnreadEntryType entryType, CancellationToken cancellationToken)
    {
        return Collection.UpdateManyAsync(
            Filter.Eq(c => c.EntityId, entityId) &
            Filter.Eq(c => c.EntryType, entryType),
            Update.Set(c => c.IsRemoved, true),
            cancellationToken: cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IDictionary<Guid, int>> SelectByParents(Guid userId, UnreadEntryType entryType,
        IReadOnlyCollection<Guid> parentIds, CancellationToken cancellationToken)
    {
        var userIds = new[] { userId, Guid.Empty }.Distinct();
        var counters = (await Collection.Aggregate()
                .Match(
                    Filter.In(c => c.UserId, userIds) &
                    Filter.In(c => c.ParentId, parentIds) &
                    Filter.Eq(c => c.EntryType, entryType) &
                    Filter.Eq(c => c.IsRemoved, false))
                .Group(c => c.EntityId,
                    g => new UnreadCounter
                    {
                        EntityId = g.First().EntityId,
                        ParentId = g.First().ParentId,
                        Counter = g.Min(c => c.Counter)
                    })
                .Group(c => c.ParentId,
                    g => new UnreadCounter
                    {
                        EntityId = g.First().ParentId,
                        Counter = g.Sum(c => c.Counter > 0 ? 1 : 0)
                    })
                .ToListAsync(cancellationToken))
            .ToDictionary(c => c.EntityId, c => c.Counter);
        return parentIds.ToDictionary(id => id, id => counters.TryGetValue(id, out var counter) ? counter : 0);
    }

    /// <inheritdoc />
    public async Task<IDictionary<Guid, int>> SelectByEntities(Guid userId, UnreadEntryType entryType,
        IReadOnlyCollection<Guid> entityIds, CancellationToken cancellationToken)
    {
        var userIds = new[] { userId, Guid.Empty }.Distinct();
        var counters = (await Collection.Aggregate()
                .Match(
                    Filter.In(c => c.UserId, userIds) &
                    Filter.In(c => c.EntityId, entityIds) &
                    Filter.Eq(c => c.EntryType, entryType) &
                    Filter.Eq(c => c.IsRemoved, false))
                .Group(c => c.EntityId,
                    g => new UnreadCounter
                    {
                        EntityId = g.First().EntityId,
                        Counter = g.Min(c => c.Counter)
                    })
                .ToListAsync(cancellationToken))
            .ToDictionary(c => c.EntityId, c => c.Counter);
        return entityIds.ToDictionary(id => id, id => counters.TryGetValue(id, out var counter) ? counter : 0);
    }

    /// <inheritdoc />
    public async Task Flush(Guid userId, UnreadEntryType entryType, Guid entityId, CancellationToken cancellationToken)
    {
        var counter = await Collection.Find(
                Filter.Eq(c => c.EntityId, entityId) &
                Filter.Eq(c => c.EntryType, entryType))
            .FirstOrDefaultAsync(cancellationToken);

        await Collection
            .ReplaceOneAsync(
                Filter.Eq(c => c.UserId, userId) &
                Filter.Eq(c => c.EntityId, entityId) &
                Filter.Eq(c => c.EntryType, entryType),
                new UnreadCounter
                {
                    UserId = userId,
                    EntityId = entityId,
                    ParentId = counter.ParentId,
                    EntryType = entryType,
                    LastRead = dateTimeProvider.Now.UtcDateTime,
                    Counter = 0
                },
                new ReplaceOptions { IsUpsert = true },
                cancellationToken);
    }

    /// <inheritdoc />
    public async Task FlushAll(Guid userId, UnreadEntryType entryType, Guid parentId,
        CancellationToken cancellationToken)
    {
        var entityIds = await Collection.Distinct(
                c => c.EntityId,
                Filter.Eq(c => c.ParentId, parentId) &
                Filter.Eq(c => c.EntryType, entryType),
                cancellationToken: cancellationToken)
            .ToListAsync(cancellationToken);
        var rightNow = dateTimeProvider.Now.UtcDateTime;

        await Collection.BulkWriteAsync(entityIds
            .Select(id => new ReplaceOneModel<UnreadCounter>(
                    Filter.Eq(c => c.UserId, userId) &
                    Filter.Eq(c => c.EntityId, id) &
                    Filter.Eq(c => c.EntryType, entryType),
                    new UnreadCounter
                    {
                        UserId = userId,
                        EntityId = id,
                        ParentId = parentId,
                        EntryType = entryType,
                        LastRead = rightNow,
                        Counter = 0
                    })
                { IsUpsert = true }), cancellationToken: cancellationToken);
    }

    /// <inheritdoc />
    public async Task ChangeParent(Guid parentId, UnreadEntryType entryType,
        Guid newParentId, CancellationToken cancellationToken)
    {
        await Collection.UpdateManyAsync(
            Filter.Eq(c => c.ParentId, parentId) &
            Filter.Eq(c => c.EntryType, entryType),
            Update.Set(c => c.ParentId, newParentId),
            cancellationToken: cancellationToken);
    }
}