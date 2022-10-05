using System.Threading.Tasks;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Users;

namespace DM.Web.API.Services.Fora;

/// <summary>
/// API service for forum moderators
/// </summary>
public interface IModeratorsApiService
{
    /// <summary>
    /// Get list of forum moderators
    /// </summary>
    /// <param name="id">Forum id</param>
    /// <returns>Envelope of moderators list</returns>
    Task<ListEnvelope<User>> GetModerators(string id);
}