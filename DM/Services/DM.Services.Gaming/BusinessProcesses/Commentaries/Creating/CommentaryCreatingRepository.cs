using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DM.Services.DataAccess;
using Microsoft.EntityFrameworkCore;
using Comment = DM.Services.DataAccess.BusinessObjects.Common.Comment;

namespace DM.Services.Gaming.BusinessProcesses.Commentaries.Creating
{
    /// <inheritdoc />
    public class CommentaryCreatingRepository : ICommentaryCreatingRepository
    {
        private readonly DmDbContext dbContext;
        private readonly IMapper mapper;

        /// <inheritdoc />
        public CommentaryCreatingRepository(
            DmDbContext dbContext,
            IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        /// <inheritdoc />
        public async Task<Services.Common.Dto.Comment> Create(Comment comment)
        {
            dbContext.Comments.Add(comment);
            await dbContext.SaveChangesAsync();
            return await dbContext.Comments
                .Where(c => c.CommentId == comment.CommentId)
                .ProjectTo<Services.Common.Dto.Comment>(mapper.ConfigurationProvider)
                .FirstAsync();
        }
    }
}