using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Services.Core.Dto.Enums;
using DM.Services.DataAccess.SearchEngine;
using DM.Services.Search.Dto;

namespace DM.Services.Search.Repositories
{
    /// <summary>
    /// Searchable entities storage
    /// </summary>
    public interface ISearchEngineRepository
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
        /// Search entities in index by text
        /// </summary>
        /// <param name="query">Search query</param>
        /// <param name="skip">Skip first entities</param>
        /// <param name="take">Take next entities</param>
        /// <param name="roles">Authenticated user roles</param>
        /// <param name="userId">Authenticated user identifier</param>
        /// <returns></returns>
        Task<(IEnumerable<FoundEntity> entities, int totalCount)> Search(string query, int skip, int take,
            IEnumerable<UserRole> roles, Guid userId);
    }
}