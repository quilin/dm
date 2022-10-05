using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DM.Services.Core.Extensions;

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

    /// <summary>
    /// Typed results for parallel task execution
    /// </summary>
    /// <returns></returns>
    public static async Task<(T1, T2)> WhenAll<T1, T2>(Task<T1> task1, Task<T2> task2)
    {
        await Task.WhenAll(task1, task2);
        return (await task1, await task2);
    }

    /// <summary>
    /// Typed results for parallel task execution
    /// </summary>
    /// <returns></returns>
    public static async Task<(T1, T2, T3)> WhenAll<T1, T2, T3>(Task<T1> task1, Task<T2> task2, Task<T3> task3)
    {
        await Task.WhenAll(task1, task2, task3);
        return (await task1, await task2, await task3);
    }

    /// <summary>
    /// Typed results for parallel task execution
    /// </summary>
    /// <returns></returns>
    public static async Task<(T1, T2, T3, T4)> WhenAll<T1, T2, T3, T4>(Task<T1> task1, Task<T2> task2, Task<T3> task3,
        Task<T4> task4)
    {
        await Task.WhenAll(task1, task2, task3, task4);
        return (await task1, await task2, await task3, await task4);
    }
}