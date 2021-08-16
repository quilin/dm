using System;
using System.Security.Cryptography;

namespace DM.Services.Authentication.Implementation.Security
{
    /// <inheritdoc />
    internal class SaltFactory : ISaltFactory
    {
        private readonly Lazy<RNGCryptoServiceProvider> rngCryptoServiceProvider = new Lazy<RNGCryptoServiceProvider>(
            () => new RNGCryptoServiceProvider());

        /// <inheritdoc />
        public string Create(int saltLength)
        {
            var size = saltLength * 4 / 3;
            var buffer = new byte[size];
            rngCryptoServiceProvider.Value.GetBytes(buffer);
            var base64String = Convert.ToBase64String(buffer);
            return base64String.Length > saltLength
                ? base64String.Substring(0, saltLength)
                : base64String;
        }
    }
}