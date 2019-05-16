using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DM.Services.DataAccess;
using DM.Services.DataAccess.BusinessObjects.Fora;
using DM.Services.DataAccess.RelationalStorage;
using DM.Services.Forum.Dto.Internal;
using Microsoft.EntityFrameworkCore;

namespace DM.Services.Forum.BusinessProcesses.Commentaries.Deleting
{
    /// <inheritdoc />
    public class CommentaryDeletingRepository : ICommentaryDeletingRepository
    {
        private readonly DmDbContext dbContext;
        private readonly IMapper mapper;

        /// <inheritdoc />
        public CommentaryDeletingRepository(
            DmDbContext dbContext,
            IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        /// <inheritdoc />
        public Task<CommentToDelete> GetForDelete(Guid commentId)
        {
            return dbContext.Comments
                .Where(c => !c.IsRemoved && c.ForumCommentId == commentId)
                .ProjectTo<CommentToDelete>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }

        /// <inheritdoc />
        public Task Delete(UpdateBuilder<ForumComment> update, UpdateBuilder<ForumTopic> topicUpdate)
        {
            update.Update(dbContext);
            return dbContext.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task<Guid?> GetSecondLastCommentId(Guid topicId)
        {
            var result = await dbContext.Comments
                .Where(c => !c.IsRemoved && c.ForumTopicId == topicId)
                .OrderByDescending(c => c.CreateDate)
                .Skip(1)
                .Select(c => c.ForumCommentId)
                .FirstOrDefaultAsync();
            return result == default ? (Guid?) null : result;
        }
    }
}