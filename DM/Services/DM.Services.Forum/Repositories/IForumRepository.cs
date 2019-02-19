using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Services.Core.Dto.Enums;

namespace DM.Services.Forum.Repositories
{
    public interface IForumRepository
    {
        Task<IEnumerable<Dto.Forum>> SelectFora(ForumAccessPolicy accessPolicy);
    }
}