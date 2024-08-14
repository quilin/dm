using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace DM.Services.Authentication.Implementation.Security;

/// <inheritdoc />
internal class HashProvider : IHashProvider
{
    private readonly Lazy<SHA256> sha256 = new(SHA256.Create);

    /// <inheritdoc />
    public byte[] ComputeSha256(string plainText, string salt)
    {
        var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
        var saltBytes = Convert.FromBase64String(salt);

        var buffer = plainTextBytes.Concat(saltBytes).ToArray();

        lock (sha256)
        {
            return sha256.Value.ComputeHash(buffer);
        }
    }
}