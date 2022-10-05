using System;
using System.Collections.Generic;
using DM.Services.Core.Dto;
using DM.Services.Core.Dto.Enums;

namespace DM.Web.API.Dto.Games;

/// <summary>
/// Input DTO for games filtering
/// </summary>
public class GamesQuery : PagingQuery
{
    /// <summary>
    /// Game statuses to filter by
    /// </summary>
    public IEnumerable<GameStatus> Statuses { get; set; }

    /// <summary>
    /// Game tags to filter by
    /// </summary>
    public IEnumerable<Guid> Tag { get; set; }
}