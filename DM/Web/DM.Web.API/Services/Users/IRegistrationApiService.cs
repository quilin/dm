using System.Threading.Tasks;
using DM.Web.API.Dto.Users;

namespace DM.Web.API.Services.Users;

/// <summary>
/// API service for registration
/// </summary>
public interface IRegistrationApiService
{
    /// <summary>
    /// Register new user
    /// </summary>
    /// <param name="registration">Registration information</param>
    Task Register(Registration registration);
}