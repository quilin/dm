using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DM.Services.DataAccess.BusinessObjects.Common;
using DM.Services.DataAccess.MongoIntegration;
using MongoDB.Driver;

namespace DM.Services.Common.Repositories
{
    public class UnreadCountersRepository : MongoRepository, IUnreadCountersRepository
    {
        public UnreadCountersRepository(DmMongoClient client) : base(client)
        {
        }

        public async Task<IDictionary<Guid, int>> SelectByParents(
            Guid userId, UnreadEntryType entryType, params Guid[] parentIds)
        {
            var userIds = new[] {userId, Guid.Empty}.Distinct();
            var counters = (await Collection<UnreadCounter>().Aggregate()
                    .Match(
                        Filter<UnreadCounter>().In(c => c.UserId, userIds) &
                        Filter<UnreadCounter>().In(c => c.ParentId, parentIds) &
                        Filter<UnreadCounter>().Eq(c => c.EntryType, entryType))
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
                    .ToListAsync())
                .ToDictionary(c => c.EntityId, c => c.Counter);
            return parentIds.ToDictionary(id => id, id => counters.TryGetValue(id, out var counter) ? counter : 0);
        }

        public async Task<IDictionary<Guid, int>> SelectByEntities(
            Guid userId, UnreadEntryType entryType, params Guid[] entityIds)
        {
            var userIds = new[] {userId, Guid.Empty}.Distinct();
            var counters = (await Collection<UnreadCounter>().Aggregate()
                    .Match(
                        Filter<UnreadCounter>().In(c => c.UserId, userIds) &
                        Filter<UnreadCounter>().In(c => c.EntityId, entityIds) &
                        Filter<UnreadCounter>().Eq(c => c.EntryType, entryType))
                    .Group(c => c.EntityId,
                        g => new UnreadCounter
                        {
                            EntityId = g.First().EntityId,
                            Counter = g.Min(c => c.Counter)
                        })
                    .ToListAsync())
                .ToDictionary(c => c.EntityId, c => c.Counter);
            return entityIds.ToDictionary(id => id, id => counters.TryGetValue(id, out var counter) ? counter : 0);
        }
    }
}