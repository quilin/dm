using System;

namespace DM.Web.API.Dto.Users;

/// <summary>
/// API DTO model for password changing
/// </summary>
public class ChangePassword
{
    /// <summary>
    /// User login
    /// </summary>
    public string Login { get; set; }

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