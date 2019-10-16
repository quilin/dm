using System.Threading.Tasks;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Users;

namespace DM.Web.API.Services.Users
{
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
    }
}