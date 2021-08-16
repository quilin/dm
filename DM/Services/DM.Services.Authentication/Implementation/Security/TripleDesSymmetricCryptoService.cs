using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DM.Services.Authentication.Implementation.Security
{
    /// <inheritdoc />
    internal class TripleDesSymmetricCryptoService : ISymmetricCryptoService
    {
        private const string Key = "QkEeenXpHqgP6tOWwpUetAFvUUZiMb4f";
        private const string Iv = "dtEzMsz2ogg=";
        private readonly Lazy<TripleDESCryptoServiceProvider> tripleDesService =
            new Lazy<TripleDESCryptoServiceProvider>(() => new TripleDESCryptoServiceProvider());

        /// <inheritdoc />
        public async Task<string> Encrypt(string valueToEncrypt)
        {
            using var encryptedStream = new MemoryStream();
            var key = Convert.FromBase64String(Key);
            var iv = Convert.FromBase64String(Iv);
            using (var stream = new CryptoStream(encryptedStream,
                tripleDesService.Value.CreateEncryptor(key, iv),
                CryptoStreamMode.Write))
            {
                var data = Encoding.UTF8.GetBytes(valueToEncrypt);
                await stream.WriteAsync(data, 0, data.Length);
            }

            return Convert.ToBase64String(encryptedStream.ToArray());
        }

        /// <inheritdoc />
        public async Task<string> Decrypt(string valueToDecrypt)
        {
            using var decryptedStream = new MemoryStream();
            var key = Convert.FromBase64String(Key);
            var iv = Convert.FromBase64String(Iv);
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