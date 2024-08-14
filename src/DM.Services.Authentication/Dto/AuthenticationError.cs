namespace DM.Services.Authentication.Dto;

/// <summary>
/// Possible error during the authentication process
/// </summary>
public enum AuthenticationError
{
    /// <summary>
    /// No error, user is authenticated
    /// </summary>
    NoError = 0,

    /// <summary>
    /// Could not find the user with given login
    /// </summary>
    WrongLogin = 1,

    /// <summary>
    /// Login was correct, but the password didn't match
    /// </summary>
    WrongPassword = 2,

    /// <summary>
    /// Credentials are correct, but the user is fully banned
    /// </summary>
    Banned = 3,

    /// <summary>
    /// Credentials are correct, but the user was never activated by email
    /// </summary>
    Inactive = 4,

    /// <summary>
    /// Credentials are correct, but the user was removed from DB
    /// </summary>
    Removed = 5,

    /// <summary>
    /// Given authentication token has expired
    /// </summary>
    SessionExpired = 6,

    /// <summary>
    /// Unknown authentication error
    /// </summary>
    Forbidden = 7,

    /// <summary>
    /// Token is supposedly forged
    /// </summary>
    ForgedToken = 8,
}