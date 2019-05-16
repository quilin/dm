using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DM.Services.Core.Extensions
{
    /// <summary>
    /// Some async extensions
    /// </summary>
    public static class AsyncExtensions
    {
        /// <summary>
        /// SelectMany for async callbacks
        /// </summary>
        /// <param name="source">Source enumerable</param>
        /// <param name="selector">Target selector</param>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TTarget"></typeparam>
        /// <returns></returns>
        public static async Task<IEnumerable<TTarget>> SelectManyAsync<TSource, TTarget>(this IEnumerable<TSource> source,
            Func<TSource, Task<IEnumerable<TTarget>>> selector)
        {
            return (await Task.WhenAll(source.Select(selector))).SelectMany(s => s);
        }
    }
}