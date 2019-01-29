using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Services.DataAccess.BusinessObjects.Common;

namespace DM.Services.Common.Repositories
{
    public interface IUnreadCountersRepository
    {
        Task<IDictionary<Guid, int>> SelectByParents(
            Guid userId, IEnumerable<Guid> parentIds, UnreadEntryType entryType);

        Task<IDictionary<Guid, int>> SelectByEntities(
            Guid userId, IEnumerable<Guid> entityIds, UnreadEntryType entryType);
    }
}