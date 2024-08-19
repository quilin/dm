using System.Threading;
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
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<string> Encrypt(string valueToEncrypt, CancellationToken cancellationToken);

    /// <summary>
    /// Decrypts stored value
    /// </summary>
    /// <param name="valueToDecrypt">Stored value</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<string> Decrypt(string valueToDecrypt, CancellationToken cancellationToken);
}