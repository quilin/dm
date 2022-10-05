using DM.Services.Core.Dto;

namespace DM.Web.API.Dto.Community;

/// <inheritdoc />
public class PollsQuery : PagingQuery
{
    /// <summary>
    /// Only get active polls
    /// </summary>
    public bool OnlyActive { get; set; }
}