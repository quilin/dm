using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DM.Services.DataAccess;
using DM.Services.DataAccess.RelationalStorage;
using Microsoft.EntityFrameworkCore;
using Comment = DM.Services.DataAccess.BusinessObjects.Common.Comment;

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
        public async Task<Dto.Output.Comment> Update(IUpdateBuilder<Comment> update)
        {
            var commentId = update.AttachTo(dbContext);
            await dbContext.SaveChangesAsync();
            return await dbContext.Comments
                .Where(c => c.CommentId == commentId)
                .ProjectTo<Dto.Output.Comment>(mapper.ConfigurationProvider)
                .FirstAsync();
        }
    }
}