using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DM.Services.Core.Dto;
using DM.Services.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace DM.Services.Forum.Repositories
{
    public class ModeratorRepository : IModeratorRepository
    {
        private readonly ReadDmDbContext dmDbContext;
        private readonly IMapper mapper;

        public ModeratorRepository(
            ReadDmDbContext dmDbContext,
            IMapper mapper)
        {
            this.dmDbContext = dmDbContext;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<GeneralUser>> Get(Guid forumId)
        {
            return await dmDbContext.ForumModerators
                .Where(m => m.ForumId == forumId)
                .Select(m => m.User)
                .ProjectTo<GeneralUser>(mapper.ConfigurationProvider)
                .ToArrayAsync();
        }
    }
}