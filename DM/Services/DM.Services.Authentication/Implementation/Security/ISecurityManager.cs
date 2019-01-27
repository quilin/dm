namespace DM.Services.Authentication.Implementation.Security
{
    public interface ISecurityManager
    {
        (string Hash, string Salt) GeneratePassword(string password);
        bool ComparePasswords(string password, string salt, string hash);
    }
}