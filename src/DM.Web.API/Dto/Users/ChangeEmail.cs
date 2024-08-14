namespace DM.Web.API.Dto.Users;

/// <summary>
/// API DTO model for changing user email
/// </summary>
public class ChangeEmail
{
    /// <summary>
    /// User login
    /// </summary>
    public string Login { get; set; }

    /// <summary>
    /// User password
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// New user email
    /// </summary>
    public string Email { get; set; }
}