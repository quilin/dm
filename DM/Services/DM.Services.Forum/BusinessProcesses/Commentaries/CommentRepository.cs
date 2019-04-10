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
using DbComment = DM.Services.DataAccess.BusinessObjects.Common.Comment;

namespace DM.Services.Forum.BusinessProcesses.Commentaries
{
    /// <inheritdoc />
    public class CommentRepository : ICommentRepository
    {
        private readonly DmDbContext dbContext;
        private readonly IMapper mapper;

        /// <inheritdoc />
        public CommentRepository(
            DmDbContext dbContext,
            IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        /// <inheritdoc />
        public Task<int> Count(Guid topicId) =>
            dbContext.Comments.CountAsync(c => !c.IsRemoved && c.EntityId == topicId);

        /// <inheritdoc />
        public async Task<IEnumerable<Comment>> Get(Guid topicId, PagingData paging)
        {
            return await dbContext.Comments
                .Where(c => !c.IsRemoved && c.EntityId == topicId)
                .OrderBy(c => c.CreateDate)
                .ProjectTo<Comment>(mapper.ConfigurationProvider)
                .ToArrayAsync();
        }
    }
}