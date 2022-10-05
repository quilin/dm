using System;

namespace DM.Web.Core.Authentication.Credentials;

/// <summary>
/// Unconditional login credentials
/// </summary>
internal class UnconditionalCredentials : AuthCredentials
{
    /// <summary>
    /// User identifier
    /// </summary>
    public Guid UserId { get; set; }
}