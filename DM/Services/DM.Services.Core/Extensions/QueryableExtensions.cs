using System.Linq;
using DM.Services.Core.Dto;

namespace DM.Services.Core.Extensions;

/// <summary>
/// Paging query utils
/// </summary>
public static class QueryableExtensions
{
    /// <summary>
    /// Skip and take
    /// </summary>
    /// <param name="queryable"></param>
    /// <param name="paging"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static IQueryable<T> Page<T>(this IOrderedQueryable<T> queryable,
        PagingData paging)
    {
        return paging == null
            ? queryable
            : queryable.Skip(paging.Skip).Take(paging.Take);
    }
}