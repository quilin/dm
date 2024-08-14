using DM.Services.Authentication.Dto;

namespace DM.Services.Community.BusinessProcesses.Users.Updating;

/// <summary>
/// DTO for user updating
/// </summary>
public class UpdateUser
{
    /// <summary>
    /// User login
    /// </summary>
    public string Login { get; set; }

    /// <summary>
    /// User defined status
    /// </summary>
    public string Status { get; set; }

    /// <summary>
    /// Rating disability flag
    /// </summary>
    public bool? RatingDisabled { get; set; }

    /// <summary>
    /// User real name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// User real location
    /// </summary>
    public string Location { get; set; }

    /// <summary>
    /// User ICQ number
    /// </summary>
    public string Icq { get; set; }

    /// <summary>
    /// User Skype login
    /// </summary>
    public string Skype { get; set; }

    /// <summary>
    /// User-defined extended information
    /// </summary>
    public string Info { get; set; }

    /// <summary>
    /// User settings
    /// </summary>
    public UserSettings Settings { get; set; }
}