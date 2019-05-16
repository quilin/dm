using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DM.Services.DataAccess;
using DM.Services.DataAccess.BusinessObjects.Fora;
using DM.Services.DataAccess.RelationalStorage;
using DM.Services.Forum.Dto.Output;
using Microsoft.EntityFrameworkCore;

namespace DM.Services.Forum.BusinessProcesses.Topics.Updating
{
    /// <inheritdoc />
    public class TopicUpdatingRepository : ITopicUpdatingRepository
    {
        private readonly DmDbContext dbContext;
        private readonly IMapper mapper;

        /// <inheritdoc />
        public TopicUpdatingRepository(
            DmDbContext dbContext,
            IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        /// <inheritdoc />
        public async Task<Topic> Update(UpdateBuilder<ForumTopic> updateBuilder)
        {
            var topicId = updateBuilder.Update(dbContext);
            await dbContext.SaveChangesAsync();
            return await dbContext.ForumTopics
                .Where(t => t.ForumTopicId == topicId)
                .ProjectTo<Topic>(mapper.ConfigurationProvider)
                .FirstAsync();
        }
    }
}