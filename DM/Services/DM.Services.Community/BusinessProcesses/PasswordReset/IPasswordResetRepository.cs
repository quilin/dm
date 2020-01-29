using System.Threading.Tasks;
using DM.Services.Core.Dto;
using DM.Services.DataAccess.BusinessObjects.Users;

namespace DM.Services.Community.BusinessProcesses.PasswordReset
{
    /// <summary>
    /// Storage for password resetting
    /// </summary>
    public interface IPasswordResetRepository
    {
        /// <summary>
        /// Find user by login
        /// </summary>
        /// <param name="login">User login</param>
        /// <returns></returns>
        Task<GeneralUser> FindUser(string login);

        /// <summary>
        /// Create password restoration token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        Task CreateToken(Token token);
    }
}