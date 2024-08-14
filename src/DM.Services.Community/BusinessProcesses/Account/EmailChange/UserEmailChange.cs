namespace DM.Services.Community.BusinessProcesses.Account.EmailChange;

/// <summary>
/// DTO for email change
/// </summary>
public class UserEmailChange
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