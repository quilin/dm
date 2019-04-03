using System.Threading.Tasks;
using DM.Services.Authentication.Dto;

namespace DM.Services.Authentication.Implementation
{
    /// <summary>
    /// Service for user registration
    /// </summary>
    public interface IRegistrationService
    {
        /// <summary>
        /// Register new user
        /// </summary>
        /// <param name="registration">Registration info</param>
        /// <returns></returns>
        Task Register(UserRegistration registration);
    }
}