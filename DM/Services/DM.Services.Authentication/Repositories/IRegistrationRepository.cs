using System;
using System.Threading;
using System.Threading.Tasks;
using DM.Services.DataAccess.BusinessObjects.Users;
using DM.Services.DataAccess.RelationalStorage;

namespace DM.Services.Authentication.Repositories
{
    /// <summary>
    /// Registration information storage
    /// </summary>
    public interface IRegistrationRepository
    {
        /// <summary>
        /// Tells if user with certain email is already registered
        /// </summary>
        /// <param name="email">Email</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> EmailFree(string email, CancellationToken cancellationToken);

        /// <summary>
        /// Tells if user with certain login is already registered
        /// </summary>
        /// <param name="login"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> LoginFree(string login, CancellationToken cancellationToken);

        /// <summary>
        /// Create new user
        /// </summary>
        /// <param name="user">User DAL</param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task AddUser(User user, Token token);

        /// <summary>
        /// Find user identifier by its activation token identifier
        /// </summary>
        /// <param name="tokenId">Token identifier</param>
        /// <param name="createdSince">Created since</param>
        /// <returns></returns>
        Task<Guid> FindUserToActivate(Guid tokenId, DateTime createdSince);

        /// <summary>
        /// Update user and its token
        /// </summary>
        /// <param name="updateUser">User update</param>
        /// <param name="updateToken">Token update</param>
        /// <returns></returns>
        Task ActivateUser(UpdateBuilder<User> updateUser, UpdateBuilder<Token> updateToken);
    }
}