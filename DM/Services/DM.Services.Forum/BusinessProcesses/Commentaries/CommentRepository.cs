using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DM.Services.Common.Dto;
using DM.Services.Core.Dto;
using DM.Services.DataAccess;
using DM.Services.DataAccess.BusinessObjects.Fora;
using DM.Services.DataAccess.RelationalStorage;
using Microsoft.EntityFrameworkCore;

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
        public Task<int> Count(Guid topicId) => dbContext.Comments
            .CountAsync(c => !c.IsRemoved && c.ForumTopicId == topicId);

        /// <inheritdoc />
        public async Task<IEnumerable<Comment>> Get(Guid topicId, PagingData paging)
        {
            return await dbContext.Comments
                .Where(c => !c.IsRemoved && c.ForumTopicId == topicId)
                .OrderBy(c => c.CreateDate)
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

        /// <inheritdoc />
        public async Task<Comment> Create(ForumComment comment)
        {
            dbContext.Comments.Add(comment);
            await dbContext.SaveChangesAsync();
            return await dbContext.Comments
                .Where(c => c.ForumCommentId == comment.ForumCommentId)
                .ProjectTo<Comment>(mapper.ConfigurationProvider)
                .FirstAsync();
        }

        /// <inheritdoc />
        public async Task<Comment> Update(Guid commentId, UpdateBuilder<ForumComment> update)
        {
            await update.Update(commentId, dbContext);
            return await dbContext.Comments
                .Where(c => c.ForumCommentId == commentId)
                .ProjectTo<Comment>(mapper.ConfigurationProvider)
                .FirstAsync();
        }
    }
}