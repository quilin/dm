namespace DM.Services.Authentication.Implementation.Security;

/// <summary>
/// Provides general operations for password security
/// </summary>
public interface ISecurityManager
{
    /// <summary>
    /// Generates pair of a hash and salt for given password
    /// </summary>
    /// <param name="password">Plain password</param>
    /// <returns>Hash and salt</returns>
    (string Hash, string Salt) GeneratePassword(string password);

    /// <summary>
    /// Compares a given password and pair of salt and hash
    /// </summary>
    /// <param name="password">Plain password</param>
    /// <param name="salt">Salt</param>
    /// <param name="hash">Hash</param>
    /// <returns>If password is correct</returns>
    bool ComparePasswords(string password, string salt, string hash);
}