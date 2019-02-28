using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Services.DataAccess.BusinessObjects.Common;

namespace DM.Services.Common.Repositories
{
    public interface IUnreadCountersRepository
    {
        Task Create(Guid entityId, Guid parentId, UnreadEntryType entryType);

        Task Increment(Guid entityId, UnreadEntryType entryType);
        Task Decrement(Guid entityId, UnreadEntryType entryType, DateTime createDate);

        Task<IDictionary<Guid, int>> SelectByParents(
            Guid userId, UnreadEntryType entryType, params Guid[] parentIds);

        Task<IDictionary<Guid, int>> SelectByEntities(
            Guid userId, UnreadEntryType entryType, params Guid[] entityIds);

        Task Flush(Guid userId, UnreadEntryType entryType, Guid entityId);
        Task FlushAll(Guid userId, UnreadEntryType entryType, Guid parentId);
    }
}