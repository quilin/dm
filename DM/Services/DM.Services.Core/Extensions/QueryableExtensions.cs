using System.Linq;

namespace DM.Services.Core.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> Page<T>(this IQueryable<T> queryable, int pageNumber, int pageSize) =>
            queryable.Skip((pageNumber - 1) * pageSize).Take(pageSize);
    }
}