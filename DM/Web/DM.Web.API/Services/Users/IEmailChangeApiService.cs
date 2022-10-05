using System.Threading.Tasks;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Users;

namespace DM.Web.API.Services.Users;

/// <summary>
/// API service for user email change
/// </summary>
public interface IEmailChangeApiService
{
    /// <summary>
    /// Change user email
    /// </summary>
    /// <param name="changeEmail"></param>
    /// <returns></returns>
    Task<Envelope<User>> Change(ChangeEmail changeEmail);
}