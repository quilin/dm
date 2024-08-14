using DM.Web.API.BbRendering;

namespace DM.Web.API.Dto.Users;

/// <summary>
/// DTO model for user details
/// </summary>
public class UserDetails : User
{
    /// <summary>
    /// User ICQ number
    /// </summary>
    public string Icq { get; set; }

    /// <summary>
    /// User Skype login
    /// </summary>
    public string Skype { get; set; }

    /// <summary>
    /// URL of profile picture original
    /// </summary>
    public string OriginalPictureUrl { get; set; }

    /// <summary>
    /// User-defined extended information
    /// </summary>
    public InfoBbText Info { get; set; }

    /// <summary>
    /// User settings
    /// </summary>
    public UserSettings Settings { get; set; }
}