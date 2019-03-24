using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DM.Services.Common.Dto;
using DM.Services.Core.Dto;
using DM.Services.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace DM.Services.Forum.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly DmDbContext dmDbContext;
        private readonly IMapper mapper;

        public CommentRepository(
            DmDbContext dmDbContext,
            IMapper mapper)
        {
            this.dmDbContext = dmDbContext;
            this.mapper = mapper;
        }

        public Task<int> Count(Guid topicId) =>
            dmDbContext.Comments.CountAsync(c => !c.IsRemoved && c.EntityId == topicId);

        public async Task<IEnumerable<Comment>> Get(Guid topicId, PagingData paging)
        {
            return await dmDbContext.Comments
                .Where(c => !c.IsRemoved && c.EntityId == topicId)
                .OrderBy(c => c.CreateDate)
                .ProjectTo<Comment>(mapper.ConfigurationProvider)
                .ToArrayAsync();
        }
    }
}