using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DM.Services.Authentication.Implementation.Security
{
    public class TripleDesSymmetricCryptoService : ISymmetricCryptoService
    {
        private readonly Lazy<TripleDESCryptoServiceProvider> tripleDesService =
            new Lazy<TripleDESCryptoServiceProvider>(() => new TripleDESCryptoServiceProvider());
        
        public async Task<string> Encrypt(string valueToEncrypt, string keyInBase64, string ivInBase64)
        {
            using (var encryptedStream = new MemoryStream())
            {
                var key = Convert.FromBase64String(keyInBase64);
                var iv = Convert.FromBase64String(ivInBase64);
                using (var stream = new CryptoStream(encryptedStream,
                    tripleDesService.Value.CreateEncryptor(key, iv),
                    CryptoStreamMode.Write))
                {
                    var data = Encoding.UTF8.GetBytes(valueToEncrypt);
                    await stream.WriteAsync(data, 0, data.Length);
                }

                return Convert.ToBase64String(encryptedStream.ToArray());
            }
        }

        public async Task<string> Decrypt(string valueToDecrypt, string keyInBase64, string ivInBase64)
        {
            using (var decryptedStream = new MemoryStream())
            {
                var key = Convert.FromBase64String(keyInBase64);
                var iv = Convert.FromBase64String(ivInBase64);
                using (var stream = new CryptoStream(decryptedStream,
                    tripleDesService.Value.CreateDecryptor(key, iv),
                    CryptoStreamMode.Write))
                {
                    var encryptedData = Convert.FromBase64String(valueToDecrypt);
                    await stream.WriteAsync(encryptedData, 0, encryptedData.Length);
                }

                return Encoding.UTF8.GetString(decryptedStream.ToArray());
            }
        }
    }
}