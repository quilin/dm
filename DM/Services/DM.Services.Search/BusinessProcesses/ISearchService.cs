using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Services.Core.Dto;
using DM.Services.Core.Dto.Enums;
using DM.Services.Search.Dto;

namespace DM.Services.Search.BusinessProcesses
{
    /// <summary>
    /// Service for search requests
    /// </summary>
    public interface ISearchService
    {
        /// <summary>
        /// Look through entities
        /// </summary>
        /// <param name="query">Search query</param>
        /// <param name="type">Entity type</param>
        /// <param name="pagingQuery">Paging query</param>
        /// <returns></returns>
        Task<(IEnumerable<FoundEntity> results, PagingResult paging)> Search(string query, SearchEntityType? type,
            PagingQuery pagingQuery);
    }
}