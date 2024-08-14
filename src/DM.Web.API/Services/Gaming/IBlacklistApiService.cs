using System;
using System.Threading.Tasks;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Users;

namespace DM.Web.API.Services.Gaming;

/// <summary>
/// API service for game blacklist
/// </summary>
public interface IBlacklistApiService
{
    /// <summary>
    /// Get list of blacklisted users for the game
    /// </summary>
    /// <param name="gameId">Game identifier</param>
    /// <returns></returns>
    Task<ListEnvelope<User>> Get(Guid gameId);

    /// <summary>
    /// Add user to the game blacklist
    /// </summary>
    /// <param name="gameId">Game identifier</param>
    /// <param name="user">User to blacklist</param>
    /// <returns></returns>
    Task<Envelope<User>> Create(Guid gameId, User user);

    /// <summary>
    /// Remove user from the blacklist
    /// </summary>
    /// <param name="gameId">Game identifier</param>
    /// <param name="login">User login</param>
    /// <returns></returns>
    Task Delete(Guid gameId, string login);
}