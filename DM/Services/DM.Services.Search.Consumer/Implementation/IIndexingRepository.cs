using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Services.Core.Dto.Enums;
using DM.Services.DataAccess.SearchEngine;

namespace DM.Services.Search.Consumer.Implementation;

/// <summary>
/// Wrapper over the ElasticSearch indexing mechanism
/// </summary>
internal interface IIndexingRepository
{
    /// <summary>
    /// Store searchable entities in index
    /// </summary>
    /// <param name="entities">Entities to store</param>
    /// <returns></returns>
    Task Index(params SearchEntity[] entities);

    /// <summary>
    /// Delete indexed document
    /// </summary>
    /// <param name="entityId">Entity identifier</param>
    /// <returns></returns>
    Task Delete(Guid entityId);

    /// <summary>
    /// Delete indexed documents by their parent entity identifier
    /// </summary>
    /// <param name="parentEntityId">Parent entity identifier</param>
    /// <returns></returns>
    Task DeleteByParent(Guid parentEntityId);

    /// <summary>
    /// Update indexed documents authorized roles by parent entity identifier
    /// </summary>
    /// <param name="parentEntityId">Parent entity identifier</param>
    /// <param name="roles">New authorized roles list</param>
    /// <returns></returns>
    Task UpdateByParent(Guid parentEntityId, IEnumerable<UserRole> roles);

    /// <summary>
    /// Update indexed documents authorized user ids by parent entity identifier
    /// </summary>
    /// <param name="parentEntityId">Parent entity identifier</param>
    /// <param name="userIds">New authorized users ids list</param>
    /// <returns></returns>
    Task UpdateByParent(Guid parentEntityId, IEnumerable<Guid> userIds);
}