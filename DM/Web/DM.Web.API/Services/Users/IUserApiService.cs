using System.Threading.Tasks;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Users;
using Microsoft.AspNetCore.Http;

namespace DM.Web.API.Services.Users;

/// <summary>
/// API service for community users
/// </summary>
public interface IUserApiService
{
    /// <summary>
    /// Get community users
    /// </summary>
    /// <param name="query">Paging query</param>
    /// <returns></returns>
    Task<ListEnvelope<User>> GetUsers(UsersQuery query);

    /// <summary>
    /// Get community user
    /// </summary>
    /// <param name="login">User login</param>
    /// <returns></returns>
    Task<Envelope<User>> GetUser(string login);

    /// <summary>
    /// Get community user details
    /// </summary>
    /// <param name="login"></param>
    /// <returns></returns>
    Task<Envelope<UserDetails>> GetUserDetails(string login);

    /// <summary>
    /// Update user
    /// </summary>
    /// <param name="login">User login</param>
    /// <param name="user">User information</param>
    /// <returns></returns>
    Task<Envelope<UserDetails>> UpdateUser(string login, UserDetails user);

    /// <summary>
    /// Upload user profile picture
    /// </summary>
    /// <param name="login">User login</param>
    /// <param name="files">Profile picture files</param>
    /// <returns></returns>
    Task<Envelope<UserDetails>> UploadProfilePicture(string login, IFormFile files);
}