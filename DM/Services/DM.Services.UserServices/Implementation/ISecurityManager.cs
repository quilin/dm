using System.Threading.Tasks;
using DM.Services.UserServices.Dto;

namespace DM.Services.UserServices.Implementation
{
    public interface ISecurityManager
    {
        (string Hash, string Salt) GeneratePassword(string password);
        bool ComparePasswords(string password, string salt, string hash);
        Task<(UserAuthenticationError Error, AuthenticatingUser User)> Authenticate(string login, string password);
    }
}