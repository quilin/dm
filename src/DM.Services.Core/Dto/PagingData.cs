using System;

namespace DM.Services.Core.Dto;

/// <summary>
/// Paging data for repositories
/// </summary>
public class PagingData
{
    /// <inheritdoc />
    public PagingData(PagingQuery query, int defaultPageSize, int totalCount)
    {
        var pageSize = Math.Max(query.Size ?? defaultPageSize, 0);
        var entityNumber = query.Number.HasValue || !query.Skip.HasValue
            ? query.Number ?? 1
            : query.Skip.Value + 1;
        Result = PagingResult.Create(totalCount, entityNumber, pageSize == 0 ? defaultPageSize : pageSize);
        Skip = query.Number.HasValue || !query.Skip.HasValue
            ? (Result.CurrentPage - 1) * Result.PageSize
            : query.Skip.Value;
        Take = pageSize;
    }

    /// <summary>
    /// Skip previous entities
    /// </summary>
    public int Skip { get; }

    /// <summary>
    /// Page size
    /// </summary>
    public int Take { get; }

    /// <summary>
    /// Paging result
    /// </summary>
    public PagingResult Result { get; }
}