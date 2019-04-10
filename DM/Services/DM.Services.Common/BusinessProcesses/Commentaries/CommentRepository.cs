using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DM.Services.Common.Dto;
using DM.Services.DataAccess;
using DM.Services.DataAccess.RelationalStorage;
using Microsoft.EntityFrameworkCore;

namespace DM.Services.Common.BusinessProcesses.Commentaries
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
        public Task<Comment> Get(Guid commentId)
        {
            return dbContext.Comments
                .Where(c => !c.IsRemoved && c.CommentId == commentId)
                .ProjectTo<Comment>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }

        /// <inheritdoc />
        public async Task<Comment> Create(DataAccess.BusinessObjects.Common.Comment comment)
        {
            dbContext.Comments.Add(comment);
            await dbContext.SaveChangesAsync();
            return await dbContext.Comments
                .Where(c => c.CommentId == comment.CommentId)
                .ProjectTo<Comment>(mapper.ConfigurationProvider)
                .FirstAsync();
        }

        /// <inheritdoc />
        public async Task<Comment> Update(Guid commentId, UpdateBuilder<DataAccess.BusinessObjects.Common.Comment> update)
        {
            await update.Update(commentId, dbContext);
            return await dbContext.Comments
                .Where(c => c.CommentId == commentId)
                .ProjectTo<Comment>(mapper.ConfigurationProvider)
                .FirstAsync();
        }
    }
}