namespace DM.Web.Core.Authentication.Credentials;

/// <summary>
/// Token credentials
/// </summary>
public class TokenCredentials : AuthCredentials
{
    /// <summary>
    /// Token
    /// </summary>
    public string Token { get; set; }
}