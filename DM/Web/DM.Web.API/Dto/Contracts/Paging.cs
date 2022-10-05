using DM.Services.Core.Dto;

namespace DM.Web.API.Dto.Contracts;

/// <summary>
/// Paging DTO model
/// </summary>
public class Paging
{
    /// <inheritdoc />
    public Paging(PagingResult pagingResult)
    {
        Pages = pagingResult.TotalPagesCount;
        Current = pagingResult.CurrentPage;
        Size = pagingResult.PageSize;
        Number = pagingResult.EntityNumber;
        Total = pagingResult.TotalEntitiesCount;
    }

    /// <summary>
    /// Total pages count
    /// </summary>
    public int Pages { get; }

    /// <summary>
    /// Current page number
    /// </summary>
    public int Current { get; }

    /// <summary>
    /// Page size
    /// </summary>
    public int Size { get; }

    /// <summary>
    /// Entity number
    /// </summary>
    public int Number { get; }

    /// <summary>
    /// Total entity count
    /// </summary>
    public int Total { get; }
}