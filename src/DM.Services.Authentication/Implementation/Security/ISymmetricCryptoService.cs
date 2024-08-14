using System.Threading.Tasks;

namespace DM.Services.Authentication.Implementation.Security;

/// <summary>
/// Service for symmetric cryptography
/// </summary>
public interface ISymmetricCryptoService
{
    /// <summary>
    /// Encrypts value for storage
    /// </summary>
    /// <param name="valueToEncrypt">Given value</param>
    /// <returns></returns>
    Task<string> Encrypt(string valueToEncrypt);

    /// <summary>
    /// Decrypts stored value
    /// </summary>
    /// <param name="valueToDecrypt">Stored value</param>
    /// <returns></returns>
    Task<string> Decrypt(string valueToDecrypt);
}