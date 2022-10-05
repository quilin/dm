using System;
using System.Collections.Generic;
using DM.Services.Core.Dto;
using DM.Services.Core.Dto.Enums;

namespace DM.Services.Gaming.Dto.Input;

/// <summary>
/// Query to search games by
/// </summary>
public class GamesQuery : PagingQuery
{
    /// <summary>
    /// Games should only be in these statuses
    /// </summary>
    public HashSet<GameStatus> Statuses { get; set; }

    /// <summary>
    /// Games should contain this tag
    /// </summary>
    public Guid? TagId { get; set; }
}