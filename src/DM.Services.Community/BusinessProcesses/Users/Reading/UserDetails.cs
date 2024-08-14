using System;
using DM.Services.Authentication.Dto;
using DM.Services.Core.Dto;

namespace DM.Services.Community.BusinessProcesses.Users.Reading;

/// <summary>
/// DTO model for user additional data
/// </summary>
public class UserDetails : GeneralUser
{
    /// <summary>
    /// Date of user registration
    /// </summary>
    public DateTimeOffset RegistrationDate { get; set; }

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