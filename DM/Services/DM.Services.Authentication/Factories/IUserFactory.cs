using DM.Services.Authentication.Dto;
using DM.Services.DataAccess.BusinessObjects.Users;

namespace DM.Services.Authentication.Factories
{
    /// <summary>
    /// Creates new user DAL models
    /// </summary>
    public interface IUserFactory
    {
        /// <summary>
        /// Create new user from registration model
        /// </summary>
        /// <param name="registration">Registration DTO model</param>
        /// <param name="salt">Password salt</param>
        /// <param name="hash">Password hash</param>
        /// <returns>DAL model for user</returns>
        User Create(UserRegistration registration, string salt, string hash);
    }
}