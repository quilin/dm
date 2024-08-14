namespace DM.Services.Authentication.Implementation.Security;

/// <summary>
/// Provides password hash
/// </summary>
internal interface IHashProvider
{
    /// <summary>
    /// Generates a byte sequence based on plain password text and its salt
    /// </summary>
    byte[] ComputeSha256(string plainText, string salt);
}