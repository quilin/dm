using System.Threading.Tasks;

namespace DM.Services.Authentication.Implementation.Security
{
    public interface ISymmetricCryptoService
    {
        Task<string> Encrypt(string valueToEncrypt, string keyInBase64, string ivInBase64);
        Task<string> Decrypt(string valueToDecrypt, string keyInBase64, string ivInBase64);
    }
}