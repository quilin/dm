using System;
using System.Threading.Tasks;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Users;

namespace DM.Web.API.Services.Users;

/// <summary>
/// API service for user activation
/// </summary>
public interface IActivationApiService
{
    /// <summary>
    /// Activate user and authenticate
    /// </summary>
    /// <param name="token">Activation token</param>
    /// <returns></returns>
    Task<Envelope<User>> Activate(Guid token);
}