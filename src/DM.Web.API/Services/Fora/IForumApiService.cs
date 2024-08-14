using System.Threading.Tasks;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Fora;

namespace DM.Web.API.Services.Fora;

/// <summary>
/// API service for forum resources
/// </summary>
public interface IForumApiService
{
    /// <summary>
    /// Get list of available fora
    /// </summary>
    /// <returns>Envelope with fora list</returns>
    Task<ListEnvelope<Forum>> Get();

    /// <summary>
    /// Get forum by id
    /// </summary>
    /// <param name="id">Forum id</param>
    /// <returns>Envelope with forum</returns>
    Task<Envelope<Forum>> Get(string id);
}