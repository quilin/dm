using System;
using DM.Services.Core.Dto.Enums;

namespace DM.Services.Core.Dto;

/// <summary>
/// DTO model for a user in almost any circumstances
/// </summary>
public class GeneralUser : IUser
{
    /// <inheritdoc />
    public Guid UserId { get; set; }

    /// <inheritdoc />
    public string Login { get; set; }

    /// <summary>
    /// For internal usage only
    /// </summary>
    public string Email { get; set; }

    /// <inheritdoc />
    public UserRole Role { get; set; }

    /// <inheritdoc />
    public AccessPolicy AccessPolicy { get; set; }

    /// <inheritdoc />
    public DateTimeOffset? LastVisitDate { get; set; }

    /// <summary>
    /// URL of current profile picture original file
    /// </summary>
    public string OriginalPictureUrl { get; set; }

    /// <summary>
    /// URL of current profile picture M-sized
    /// </summary>
    public string MediumPictureUrl { get; set; }

    /// <summary>
    /// URL of current profile picture S-sized
    /// </summary>
    public string SmallPictureUrl { get; set; }

    /// <summary>
    /// Status
    /// </summary>
    public string Status { get; set; }

    /// <summary>
    /// Name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Location
    /// </summary>
    public string Location { get; set; }

    /// <inheritdoc />
    public bool RatingDisabled { get; set; }

    /// <inheritdoc />
    public int QualityRating { get; set; }

    /// <inheritdoc />
    public int QuantityRating { get; set; }

    /// <summary>
    /// Whether user is authenticated or not
    /// </summary>
    public bool IsAuthenticated => Role != UserRole.Guest;
}