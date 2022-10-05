using System;
using DM.Services.Core.Dto.Enums;

namespace DM.Services.Core.Dto;

/// <summary>
/// General DTO model for the user
/// </summary>
public interface IUser
{
    /// <summary>
    /// Id
    /// </summary>
    Guid UserId { get; }

    /// <summary>
    /// Login
    /// </summary>
    string Login { get; }

    /// <summary>
    /// Role
    /// </summary>
    UserRole Role { get; }

    /// <summary>
    /// Computed access restrictions
    /// </summary>
    AccessPolicy AccessPolicy { get; }

    /// <summary>
    /// Last time user performed any action on any site
    /// </summary>
    DateTimeOffset? LastVisitDate { get; }

    /// <summary>
    /// Rating participation flag
    /// </summary>
    bool RatingDisabled { get; set; }

    /// <summary>
    /// Sum of positive and negative votes for user's posts
    /// </summary>
    int QualityRating { get; set; }

    /// <summary>
    /// Total number of user's posts
    /// </summary>
    int QuantityRating { get; set; }
}