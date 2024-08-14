using System;
using System.Text;

namespace DM.Services.Authentication.Implementation.Security;

/// <inheritdoc />
internal class SecurityManager : ISecurityManager
{
    private readonly ISaltFactory saltFactory;
    private readonly IHashProvider hashProvider;

    /// <inheritdoc />
    public SecurityManager(
        ISaltFactory saltFactory,
        IHashProvider hashProvider)
    {
        this.saltFactory = saltFactory;
        this.hashProvider = hashProvider;
    }

    /// <inheritdoc />
    public (string Hash, string Salt) GeneratePassword(string password)
    {
        var salt = saltFactory.Create(100);
        var hash = hashProvider.ComputeSha256(password, salt);
        return (Convert.ToBase64String(hash), salt);
    }

    /// <inheritdoc />
    public bool ComparePasswords(string password, string salt, string hash)
    {
        var saltedHash = hashProvider.ComputeSha256(password, salt);
        var passwordHashByteArray = Convert.FromBase64String(hash);
        return string.Equals(
            Encoding.UTF8.GetString(saltedHash),
            Encoding.UTF8.GetString(passwordHashByteArray));
    }
}