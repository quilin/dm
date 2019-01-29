using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DM.Services.DataAccess.BusinessObjects.Common;
using DM.Services.DataAccess.MongoIntegration;
using MongoDB.Driver;

namespace DM.Services.Common.Repositories
{
    public class UnreadCountersRepository : MongoRepository<UnreadCounter>, IUnreadCountersRepository
    {
        public UnreadCountersRepository(DmMongoClient client) : base(client)
        {
        }

        public async Task<IDictionary<Guid, int>> SelectByParents(
            Guid userId, IEnumerable<Guid> parentIds, UnreadEntryType entryType)
        {
            var userIds = new[] {userId, Guid.Empty}.Distinct();
            var ids = parentIds.ToArray();
            var counters = (await Collection.Aggregate()
                    .Match(
                        Filter.In(c => c.UserId, userIds) &
                        Filter.In(c => c.ParentId, ids) &
                        Filter.Eq(c => c.EntryType, entryType))
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
            return ids.ToDictionary(id => id, id => counters.TryGetValue(id, out var counter) ? counter : 0);
        }

        public async Task<IDictionary<Guid, int>> SelectByEntities(
            Guid userId, IEnumerable<Guid> entityIds, UnreadEntryType entryType)
        {
            var userIds = new[] {userId, Guid.Empty}.Distinct();
            var ids = entityIds.ToArray();
            var counters = (await Collection.Aggregate()
                    .Match(
                        Filter.In(c => c.UserId, userIds) &
                        Filter.In(c => c.EntityId, ids) &
                        Filter.Eq(c => c.EntryType, entryType))
                    .Group(c => c.EntityId,
                        g => new UnreadCounter
                        {
                            EntityId = g.First().EntityId,
                            Counter = g.Min(c => c.Counter)
                        })
                    .ToListAsync())
                .ToDictionary(c => c.EntityId, c => c.Counter);
            return ids.ToDictionary(id => id, id => counters.TryGetValue(id, out var counter) ? counter : 0);
        }
    }
}