using System;
using System.Collections.Generic;
using DM.Services.Core.Dto.Enums;

namespace DM.Web.API.Dto.Users;

/// <summary>
/// DTO model for user
/// </summary>
public class User
{
    /// <summary>
    /// Login
    /// </summary>
    public string Login { get; set; }

    /// <summary>
    /// Roles
    /// </summary>
    public IEnumerable<UserRole> Roles { get; set; }

    /// <summary>
    /// Profile picture URL M-size
    /// </summary>
    public string MediumPictureUrl { get; set; }

    /// <summary>
    /// Profile picture URL S-size
    /// </summary>
    public string SmallPictureUrl { get; set; }

    /// <summary>
    /// User defined status
    /// </summary>
    public string Status { get; set; }

    /// <summary>
    /// Rating
    /// </summary>
    public Rating Rating { get; set; }

    /// <summary>
    /// Last seen online moment
    /// </summary>
    public DateTimeOffset? Online { get; set; }

    /// <summary>
    /// User real name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// User real location
    /// </summary>
    public string Location { get; set; }

    /// <summary>
    /// User registration moment
    /// </summary>
    public DateTimeOffset? Registration { get; set; }
}

/// <summary>
/// DTO model for user rating
/// </summary>
public class Rating
{
    /// <summary>
    /// Rating participation flag
    /// </summary>
    public bool Enabled { get; set; }

    /// <summary>
    /// Quality rating
    /// </summary>
    public int Quality { get; set; }

    /// <summary>
    /// Quantity rating
    /// </summary>
    public int Quantity { get; set; }
}