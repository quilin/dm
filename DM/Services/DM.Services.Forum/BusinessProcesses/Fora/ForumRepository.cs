using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DM.Services.Core.Dto.Enums;
using DM.Services.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace DM.Services.Forum.BusinessProcesses.Fora
{
    /// <inheritdoc />
    internal class ForumRepository : IForumRepository
    {
        private readonly DmDbContext dmDbContext;
        private readonly IMapper mapper;

        public ForumRepository(
            DmDbContext dmDbContext,
            IMapper mapper)
        {
            this.dmDbContext = dmDbContext;
            this.mapper = mapper;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Dto.Forum>> SelectFora(ForumAccessPolicy? accessPolicy)
        {
            var query = dmDbContext.Fora.AsQueryable();
            if (accessPolicy.HasValue)
            {
                query = query.Where(f => (f.ViewPolicy & accessPolicy) != ForumAccessPolicy.NoOne);
            }

            return await query
                .OrderBy(f => f.Order)
                .ProjectTo<Dto.Forum>(mapper.ConfigurationProvider)
                .ToArrayAsync();
        }
    }
}