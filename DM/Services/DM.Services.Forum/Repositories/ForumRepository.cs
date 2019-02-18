using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
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
        private readonly IMapper mapper;

        public ForumRepository(
            ReadDmDbContext dmDbContext,
            IMapper mapper)
        {
            this.dmDbContext = dmDbContext;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<ForaListItem>> SelectFora(ForumAccessPolicy accessPolicy)
        {
            return await dmDbContext.Fora
                .Where(f => (f.ViewPolicy & accessPolicy) != ForumAccessPolicy.NoOne)
                .OrderBy(f => f.Order)
                .ProjectTo<ForaListItem>(mapper.ConfigurationProvider)
                .ToArrayAsync();
        }
    }
}