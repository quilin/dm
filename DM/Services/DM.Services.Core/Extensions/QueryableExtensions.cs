using System.Linq;
using DM.Services.Core.Dto;

namespace DM.Services.Core.Extensions
{
    /// <summary>
    /// Paging query utils
    /// </summary>
    public static class QueryableExtensions
    {
        /// <summary>
        /// Skip and take for certain paging data
        /// </summary>
        /// <param name="queryable">Query</param>
        /// <param name="paging">Paging data</param>
        /// <typeparam name="T">Entity type</typeparam>
        /// <returns>Modified query</returns>
        public static IQueryable<T> Page<T>(this IOrderedQueryable<T> queryable, PagingData paging) =>
            paging == null
                ? queryable
                : queryable.Skip((paging.CurrentPage - 1) * paging.PageSize).Take(paging.PageSize);
    }
}