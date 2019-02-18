using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DM.Services.Core.Dto.Enums;
using DM.Services.DataAccess;
using DM.Services.Forum.Dto;
using Microsoft.EntityFrameworkCore;
using DbForum = DM.Services.DataAccess.BusinessObjects.Fora.Forum;

namespace DM.Services.Forum.Repositories
{
    internal class ForumRepository : IForumRepository
    {
        private readonly ReadDmDbContext dmDbContext;

        public ForumRepository(
            ReadDmDbContext dmDbContext)
        {
            this.dmDbContext = dmDbContext;
        }

        public async Task<IEnumerable<ForaListItem>> SelectFora(ForumAccessPolicy accessPolicy)
        {
            return await dmDbContext.Fora
                .Where(f => (f.ViewPolicy & accessPolicy) != ForumAccessPolicy.NoOne)
                .OrderBy(f => f.Order)
                .Select(f => new ForaListItem
                {
                    Id = f.ForumId,
                    Title = f.Title,
                    UnreadTopicsCount = 0
                })
                .ToArrayAsync();
        }
    }
}