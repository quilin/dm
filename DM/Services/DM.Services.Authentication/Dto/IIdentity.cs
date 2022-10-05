namespace DM.Services.Authentication.Dto;

/// <summary>
/// Authentication result provider
/// </summary>
public interface IIdentity
{
    /// <summary>
    /// Current authenticated user
    /// </summary>
    AuthenticatedUser User { get; }

    /// <summary>
    /// Current authentication session
    /// </summary>
    Session Session { get; }

    /// <summary>
    /// Current authenticated user settings
    /// </summary>
    UserSettings Settings { get; }

    /// <summary>
    /// Current authentication error state
    /// </summary>
    AuthenticationError Error { get; }

    /// <summary>
    /// Current authentication token, provided or generated
    /// </summary>
    string AuthenticationToken { get; }
}