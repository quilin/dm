using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DM.Services.DataAccess;
using DM.Services.DataAccess.BusinessObjects.Fora;
using DM.Services.DataAccess.RelationalStorage;
using DM.Services.Forum.Dto.Output;
using Microsoft.EntityFrameworkCore;

namespace DM.Services.Forum.BusinessProcesses.Commentaries.Updating
{
    /// <inheritdoc />
    public class CommentaryUpdatingRepository : ICommentaryUpdatingRepository
    {
        private readonly DmDbContext dbContext;
        private readonly IMapper mapper;

        /// <inheritdoc />
        public CommentaryUpdatingRepository(
            DmDbContext dbContext,
            IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }
        
        /// <inheritdoc />
        public async Task<Comment> Update(IUpdateBuilder<ForumComment> update)
        {
            var commentId = update.AttachTo(dbContext);
            await dbContext.SaveChangesAsync();
            return await dbContext.Comments
                .Where(c => c.ForumCommentId == commentId)
                .ProjectTo<Comment>(mapper.ConfigurationProvider)
                .FirstAsync();
        }
    }
}