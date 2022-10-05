using System;
using DM.Services.DataAccess.BusinessObjects.Users;

namespace DM.Services.DataAccess.BusinessObjects.DataContracts;

/// <summary>
/// Administrative activity contract
/// </summary>
internal interface IAdministrated : IRemovable
{
    /// <summary>
    /// Administrative activity object identifier
    /// </summary>
    Guid UserId { get; set; }

    /// <summary>
    /// Administrative activity subject identifier
    /// </summary>
    Guid ModeratorId { get; set; }

    /// <summary>
    /// Administrative activity object
    /// </summary>
    User User { get; set; }

    /// <summary>
    /// Administrative activity subject
    /// </summary>
    User Moderator { get; set; }
}