using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Services.Core.Dto.Enums;
using DM.Services.Search.Dto;

namespace DM.Services.Search.Repositories
{
    /// <summary>
    /// Searchable entities storage
    /// </summary>
    public interface ISearchEngineRepository
    {
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