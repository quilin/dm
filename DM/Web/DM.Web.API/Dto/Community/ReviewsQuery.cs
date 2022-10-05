using DM.Services.Core.Dto;

namespace DM.Web.API.Dto.Community;

/// <summary>
/// Input DTO for reviews filtering
/// </summary>
public class ReviewsQuery : PagingQuery
{
    /// <summary>
    /// Only return approved reviews
    /// </summary>
    public bool OnlyApproved { get; set; }
}