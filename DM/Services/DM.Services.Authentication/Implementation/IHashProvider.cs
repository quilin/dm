using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace DM.Services.Authentication.Implementation
{
    public interface IHashProvider
    {
        byte[] ComputeSha256(string plainText, string salt);
    }

    public class HashProvider : IHashProvider
    {
        private readonly Lazy<SHA256> sha256 = new Lazy<SHA256>(SHA256.Create);
        
        public byte[] ComputeSha256(string plainText, string salt)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            var saltBytes = Convert.FromBase64String(salt);

            var buffer = plainTextBytes.Concat(saltBytes).ToArray();

            try
            {
                lock (sha256)
                {
                    return sha256.Value.ComputeHash(buffer);
                }
            }
            catch (Exception e)
            {
                throw new HashProviderException(buffer, e);
            }
        }
    }
}