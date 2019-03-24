using System.Threading.Tasks;

namespace DM.Services.Authentication.Implementation.Security
{
    /// <summary>
    /// Service for symmetric cryptography
    /// </summary>
    public interface ISymmetricCryptoService
    {
        /// <summary>
        /// Encrypts value for storage
        /// </summary>
        /// <param name="valueToEncrypt">Given value</param>
        /// <param name="keyInBase64">Key</param>
        /// <param name="ivInBase64">Iv</param>
        /// <returns></returns>
        Task<string> Encrypt(string valueToEncrypt, string keyInBase64, string ivInBase64);

        /// <summary>
        /// Decrypts stored value
        /// </summary>
        /// <param name="valueToDecrypt">Stored value</param>
        /// <param name="keyInBase64">Key</param>
        /// <param name="ivInBase64">Iv</param>
        /// <returns></returns>
        Task<string> Decrypt(string valueToDecrypt, string keyInBase64, string ivInBase64);
    }
}