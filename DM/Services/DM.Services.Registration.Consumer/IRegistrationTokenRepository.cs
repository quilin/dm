using System;
using System.Threading.Tasks;
using DM.Services.DataAccess.BusinessObjects.Users;

namespace DM.Services.Registration.Consumer
{
    /// <summary>
    /// Registration token storage
    /// </summary>
    public interface IRegistrationTokenRepository
    {
        /// <summary>
        /// Store token
        /// </summary>
        /// <param name="token">Token</param>
        /// <returns></returns>
        Task Add(Token token);

        /// <summary>
        /// Get user email
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <returns></returns>
        Task<string> GetRegisteredUserEmail(Guid userId);
    }
}