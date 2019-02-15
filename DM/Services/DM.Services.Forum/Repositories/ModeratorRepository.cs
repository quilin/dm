using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DM.Services.Common.Repositories;
using DM.Services.Core.Dto;
using DM.Services.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace DM.Services.Forum.Repositories
{
    public class ModeratorRepository : IModeratorRepository
    {
        private readonly ReadDmDbContext dmDbContext;

        public ModeratorRepository(
            ReadDmDbContext dmDbContext)
        {
            this.dmDbContext = dmDbContext;
        }

        public async Task<IEnumerable<GeneralUser>> Get(Guid forumId)
        {
            return await dmDbContext.ForumModerators
                .Where(m => m.ForumId == forumId)
                .Select(m => m.User)
                .Select(Users.GeneralProjection)
                .ToArrayAsync();
        }
    }
}