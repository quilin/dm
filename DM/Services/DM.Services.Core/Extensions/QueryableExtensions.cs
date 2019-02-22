using System.Linq;
using DM.Services.Core.Dto;

namespace DM.Services.Core.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> Page<T>(this IQueryable<T> queryable, PagingData paging) =>
            paging == null
                ? queryable
                : queryable.Skip((paging.CurrentPage - 1) * paging.PageSize).Take(paging.PageSize);
    }
}