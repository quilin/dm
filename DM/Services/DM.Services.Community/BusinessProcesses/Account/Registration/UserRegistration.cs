namespace DM.Services.Community.BusinessProcesses.Account.Registration;

/// <summary>
/// DTO for new user registration
/// </summary>
public class UserRegistration
{
    /// <summary>
    /// Login
    /// </summary>
    public string Login { get; set; }

    /// <summary>
    /// Email
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Password
    /// </summary>
    public string Password { get; set; }
}