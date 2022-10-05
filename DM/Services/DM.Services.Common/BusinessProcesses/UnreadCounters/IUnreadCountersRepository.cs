using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Services.DataAccess.BusinessObjects.Common;

namespace DM.Services.Common.BusinessProcesses.UnreadCounters;

/// <summary>
/// Unread counters storage
/// </summary>
public interface IUnreadCountersRepository
{
    /// <summary>
    /// Create a counter for the entity for a certain user
    /// </summary>
    /// <param name="entityId">Entity Id</param>
    /// <param name="entryType">Entry type</param>
    /// <param name="userIds">User Ids</param>
    /// <returns></returns>
    Task Create(Guid entityId, UnreadEntryType entryType, IEnumerable<Guid> userIds);
    
    /// <summary>
    /// Create a counter for the entity
    /// </summary>
    /// <param name="entityId">Entity Id</param>
    /// <param name="parentId">Parent entity Id</param>
    /// <param name="entryType">Entry type</param>
    /// <returns></returns>
    Task Create(Guid entityId, Guid parentId, UnreadEntryType entryType);

    /// <summary>
    /// Create a counter for the entity without parent
    /// </summary>
    /// <param name="entityId">Entity id</param>
    /// <param name="entryType">Entry type</param>
    /// <returns></returns>
    Task Create(Guid entityId, UnreadEntryType entryType);

    /// <summary>
    /// Increment counter of the entity for every user
    /// </summary>
    /// <param name="entityId">Entity Id</param>
    /// <param name="entryType">Entry type</param>
    /// <returns></returns>
    Task Increment(Guid entityId, UnreadEntryType entryType);

    /// <summary>
    /// Decrement counter for users who hasn't read the entry since given time
    /// </summary>
    /// <param name="entityId">Entity Id</param>
    /// <param name="entryType">Entry type</param>
    /// <param name="createDate">Given time</param>
    /// <returns></returns>
    Task Decrement(Guid entityId, UnreadEntryType entryType, DateTimeOffset createDate);

    /// <summary>
    /// Remove counter for everyone
    /// </summary>
    /// <param name="entityId">Entity Id</param>
    /// <param name="entryType">Entry type</param>
    /// <returns></returns>
    Task Delete(Guid entityId, UnreadEntryType entryType);

    /// <summary>
    /// Get count of entities that have unread entries by parent entity ids
    /// </summary>
    /// <param name="userId">User Id</param>
    /// <param name="entryType">Entry type</param>
    /// <param name="parentIds">Parent entity Ids</param>
    /// <returns>List of pairs of parent entity Id and number of according entities that have unread entries</returns>
    Task<IDictionary<Guid, int>> SelectByParents(
        Guid userId, UnreadEntryType entryType, params Guid[] parentIds);

    /// <summary>
    /// Get count of unread entries by entry ids
    /// </summary>
    /// <param name="userId">User Id</param>
    /// <param name="entryType">Entry type</param>
    /// <param name="entityIds">Entity Ids</param>
    /// <returns>List of pairs of entity Id and number of entries unread</returns>
    Task<IDictionary<Guid, int>> SelectByEntities(
        Guid userId, UnreadEntryType entryType, params Guid[] entityIds);

    /// <summary>
    /// Mark entity as read
    /// </summary>
    /// <param name="userId">User Id</param>
    /// <param name="entryType">Entry type</param>
    /// <param name="entityId">Entity Id</param>
    /// <returns></returns>
    Task Flush(Guid userId, UnreadEntryType entryType, Guid entityId);

    /// <summary>
    /// Mark all according entities as read
    /// </summary>
    /// <param name="userId">User Id</param>
    /// <param name="entryType">Entry type</param>
    /// <param name="parentId">Parent entity Id</param>
    /// <returns></returns>
    Task FlushAll(Guid userId, UnreadEntryType entryType, Guid parentId);

    /// <summary>
    /// Move unread counters to new parent
    /// </summary>
    /// <param name="parentId">Parent Id</param>
    /// <param name="entryType">Entry type</param>
    /// <param name="newParentId">New parent Id</param>
    /// <returns></returns>
    Task ChangeParent(Guid parentId, UnreadEntryType entryType, Guid newParentId);
}