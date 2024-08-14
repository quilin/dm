using System;

namespace DM.Services.Community.BusinessProcesses.Account.PasswordChange;

/// <summary>
/// DTO model for password update
/// </summary>
public class UserPasswordChange
{
    /// <summary>
    /// Password reset token
    /// </summary>
    public Guid? Token { get; set; }

    /// <summary>
    /// Old password
    /// </summary>
    public string OldPassword { get; set; }

    /// <summary>
    /// New password
    /// </summary>
    public string NewPassword { get; set; }
}