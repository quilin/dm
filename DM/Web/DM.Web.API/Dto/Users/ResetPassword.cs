namespace DM.Web.API.Dto.Users;

/// <summary>
/// DTO model for user password reseting
/// </summary>
public class ResetPassword
{
    /// <summary>
    /// Login
    /// </summary>
    public string Login { get; set; }

    /// <summary>
    /// Email
    /// </summary>
    public string Email { get; set; }
}