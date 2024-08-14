using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DM.Services.Core.Dto.Enums;
using DM.Services.DataAccess.BusinessObjects.DataContracts;
using DM.Services.DataAccess.BusinessObjects.Users;

namespace DM.Services.DataAccess.BusinessObjects.Administration;

/// <summary>
/// DAL model for user ban
/// </summary>
[Table("Bans")]
public class Ban : IAdministrated
{
    /// <summary>
    /// Ban identifier
    /// </summary>
    [Key]
    public Guid BanId { get; set; }

    /// <inheritdoc />
    public Guid UserId { get; set; }

    /// <inheritdoc />
    public Guid ModeratorId { get; set; }

    /// <summary>
    /// Moment from
    /// </summary>
    public DateTimeOffset StartDate { get; set; }

    /// <summary>
    /// Moment to
    /// </summary>
    public DateTimeOffset EndDate { get; set; }

    /// <summary>
    /// Moderator commentary for the ban
    /// </summary>
    public string Comment { get; set; }

    /// <summary>
    /// Restriction policy for banned user
    /// </summary>
    public AccessPolicy AccessRestrictionPolicy { get; set; }

    /// <summary>
    /// Flag that displays that user asked to be banned
    /// </summary>
    public bool IsVoluntary { get; set; }

    /// <inheritdoc />
    public bool IsRemoved { get; set; }

    /// <inheritdoc />
    [ForeignKey(nameof(UserId))]
    public virtual User User { get; set; }

    /// <inheritdoc />
    [ForeignKey(nameof(ModeratorId))]
    public virtual User Moderator { get; set; }
}