using System;
using DM.Services.DataAccess.BusinessObjects.Users;

namespace DM.Services.Community.BusinessProcesses.PasswordReset
{
    /// <summary>
    /// Factory for password reset token
    /// </summary>
    public interface IPasswordResetTokenFactory
    {
        /// <summary>
        /// Create new token for password resetting
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <returns></returns>
        Token Create(Guid userId);
    }
}