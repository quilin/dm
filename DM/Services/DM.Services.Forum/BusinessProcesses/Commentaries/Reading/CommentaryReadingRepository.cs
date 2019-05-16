using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DM.Services.Core.Dto;
using DM.Services.Core.Extensions;
using DM.Services.DataAccess;
using DM.Services.Forum.Dto.Output;
using Microsoft.EntityFrameworkCore;

namespace DM.Services.Forum.BusinessProcesses.Commentaries.Reading

{
    /// <inheritdoc />
    public class CommentaryReadingRepository : ICommentaryReadingRepository
    {
        private readonly DmDbContext dbContext;
        private readonly IMapper mapper;

        /// <inheritdoc />
        public CommentaryReadingRepository(
            DmDbContext dbContext,
            IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        /// <inheritdoc />
        public Task<int> Count(Guid topicId) => dbContext.Comments
            .CountAsync(c => !c.IsRemoved && c.ForumTopicId == topicId);

        /// <inheritdoc />
        public async Task<IEnumerable<Comment>> Get(Guid topicId, PagingData paging)
        {
            return await dbContext.Comments
                .Where(c => !c.IsRemoved && c.ForumTopicId == topicId)
                .OrderBy(c => c.CreateDate)
                .Page(paging)
                .ProjectTo<Comment>(mapper.ConfigurationProvider)
                .ToArrayAsync();
        }

        /// <inheritdoc />
        public Task<Comment> Get(Guid commentId)
        {
            return dbContext.Comments
                .Where(c => !c.IsRemoved && c.ForumCommentId == commentId)
                .ProjectTo<Comment>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }
    }
}