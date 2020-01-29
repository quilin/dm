using System;
using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Services.DataAccess.BusinessObjects.Users;
using DM.Services.DataAccess.RelationalStorage;

namespace DM.Services.Community.BusinessProcesses.PasswordChange
{
    /// <summary>
    /// Storage for password change
    /// </summary>
    public interface IPasswordChangeRepository
    {
        /// <summary>
        /// Find existing user
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        Task<AuthenticatedUser> FindUser(string login);

        /// <summary>
        /// Check if token is valid
        /// </summary>
        /// <param name="tokenId"></param>
        /// <param name="userId"></param>
        /// <param name="createdSince"></param>
        /// <returns></returns>
        Task<bool> TokenValid(Guid tokenId, Guid userId, DateTimeOffset createdSince);

        /// <summary>
        /// Save user password changes
        /// </summary>
        /// <param name="userUpdate"></param>
        /// <param name="tokenUpdate"></param>
        /// <returns></returns>
        Task UpdatePassword(IUpdateBuilder<User> userUpdate, IUpdateBuilder<Token> tokenUpdate);
    }
}