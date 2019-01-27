namespace DM.Services.Authentication.Implementation.Security
{
    public interface IHashProvider
    {
        byte[] ComputeSha256(string plainText, string salt);
    }
}