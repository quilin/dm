using System;
using System.Threading.Tasks;

namespace DM.Services.Core.Caching;

/// <summary>
/// Cache wrapper for different strategies
/// </summary>
public interface ICache
{
    /// <summary>
    /// Get cache entry or create new and return result
    /// </summary>
    /// <param name="key">Cache key</param>
    /// <param name="create">Entry factory</param>
    /// <typeparam name="TEntry">Cache entry type</typeparam>
    /// <returns>Stored entry</returns>
    Task<TEntry> GetOrCreate<TEntry>(object key, Func<Task<TEntry>> create);

    /// <summary>
    /// Invalidate cache entry
    /// </summary>
    /// <param name="key">Cache key</param>
    /// <returns></returns>
    Task Invalidate(object key);
}