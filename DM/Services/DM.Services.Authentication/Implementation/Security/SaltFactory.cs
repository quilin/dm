using System;
using System.Security.Cryptography;

namespace DM.Services.Authentication.Implementation.Security;

/// <inheritdoc />
internal class SaltFactory : ISaltFactory
{
    /// <inheritdoc />
    public string Create(int saltLength)
    {
        var size = saltLength * 4 / 3;
        var buffer = RandomNumberGenerator.GetBytes(size);
        var base64String = Convert.ToBase64String(buffer);
        return base64String.Length > saltLength
            ? base64String[..saltLength]
            : base64String;
    }
}