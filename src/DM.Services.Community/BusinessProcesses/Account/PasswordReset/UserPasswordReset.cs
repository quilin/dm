namespace DM.Services.Community.BusinessProcesses.Account.PasswordReset;

/// <summary>
/// DTO model for password reset
/// </summary>
public class UserPasswordReset
{
    /// <summary>
    /// User login
    /// </summary>
    public string Login { get; set; }

    /// <summary>
    /// User email
    /// </summary>
    public string Email { get; set; }
}