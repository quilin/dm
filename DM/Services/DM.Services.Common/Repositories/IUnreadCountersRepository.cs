using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Services.DataAccess.BusinessObjects.Common;

namespace DM.Services.Common.Repositories
{
    public interface IUnreadCountersRepository
    {
        Task<IDictionary<Guid, int>> SelectByParents(
            Guid userId, UnreadEntryType entryType, params Guid[] parentIds);

        Task<IDictionary<Guid, int>> SelectByEntities(
            Guid userId, UnreadEntryType entryType, params Guid[] entityIds);
    }
}