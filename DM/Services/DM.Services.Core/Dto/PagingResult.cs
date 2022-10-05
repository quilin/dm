using System;

namespace DM.Services.Core.Dto;

/// <summary>
/// DTO model for paged data
/// </summary>
public class PagingResult
{
    /// <summary>
    /// Total pages of certain size across the filtered entities
    /// </summary>
    public int TotalPagesCount { get; private set; }

    /// <summary>
    /// Total entities count of certain type across the filtered entities
    /// </summary>
    public int TotalEntitiesCount { get; private set; }

    /// <summary>
    /// Current page number
    /// </summary>
    public int CurrentPage { get; private set; }

    /// <summary>
    /// Page size
    /// </summary>
    public int PageSize { get; private set; }

    /// <summary>
    /// Selected entity number
    /// </summary>
    public int EntityNumber { get; private set; }

    /// <summary>
    /// Create paging data
    /// </summary>
    /// <param name="totalEntitiesCount">Total entities count</param>
    /// <param name="entityNumber">Selected entity number</param>
    /// <param name="pageSize">Page size</param>
    /// <returns></returns>
    public static PagingResult Create(int totalEntitiesCount, int entityNumber, int pageSize)
    {
        return new PagingResult
        {
            TotalPagesCount = (int) Math.Ceiling((decimal) totalEntitiesCount / pageSize),
            TotalEntitiesCount = totalEntitiesCount,
            CurrentPage = Math.Max(1, (int) Math.Ceiling((decimal) entityNumber / pageSize)),
            PageSize = pageSize,
            EntityNumber = Math.Min(Math.Max(1, entityNumber), totalEntitiesCount)
        };
    }

    /// <summary>
    /// Empty paging result
    /// </summary>
    public static PagingResult Empty(int pageSize) => new()
    {
        TotalPagesCount = 0,
        TotalEntitiesCount = 0,
        CurrentPage = 1,
        PageSize = pageSize,
        EntityNumber = 0
    };
}