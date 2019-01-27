using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Web.Core.Authentication
{
    public class TripleDesSymmetricCryptoService : ISymmetricCryptoService
    {
        private readonly TripleDESCryptoServiceProvider tripleDesCryptoServiceProvider;

        public TripleDesSymmetricCryptoService()
        {
            tripleDesCryptoServiceProvider = new TripleDESCryptoServiceProvider();
        }

        public string Encrypt(string valueToEncrypt, string keyInBase64, string ivInBase64)
        {
            using (var encryptedStream = new MemoryStream())
            {
                var key = Convert.FromBase64String(keyInBase64);
                var iv = Convert.FromBase64String(ivInBase64);
                using (var cryptedStream = new CryptoStream(encryptedStream,
                                                            tripleDesCryptoServiceProvider.CreateEncryptor(key, iv),
                                                            CryptoStreamMode.Write))
                {
                    var data = Encoding.UTF8.GetBytes(valueToEncrypt);
                    cryptedStream.Write(data, 0, data.Length);
                }

                return Convert.ToBase64String(encryptedStream.ToArray());
            }
        }

        public string Decrypt(string valueToDecrypt, string keyInBase64, string ivInBase64)
        {
            using (var decryptedStream = new MemoryStream())
            {
                var key = Convert.FromBase64String(keyInBase64);
                var iv = Convert.FromBase64String(ivInBase64);
                using (var cryptedStream = new CryptoStream(decryptedStream,
                                                            tripleDesCryptoServiceProvider.CreateDecryptor(key, iv),
                                                            CryptoStreamMode.Write))
                {
                    var encryptedData = Convert.FromBase64String(valueToDecrypt);
                    cryptedStream.Write(encryptedData, 0, encryptedData.Length);
                }

                return Encoding.UTF8.GetString(decryptedStream.ToArray());
            }
        }
    }
}