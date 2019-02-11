using System.Threading.Tasks;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Fora;

namespace DM.Web.API.Services.Fora
{
    public interface IForumApiService
    {
        Task<ListEnvelope<Forum>> Get();
        Task<Envelope<Forum>> Get(string id);
    }
}