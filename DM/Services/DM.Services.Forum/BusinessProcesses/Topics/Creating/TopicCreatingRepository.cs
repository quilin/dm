using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DM.Services.DataAccess;
using DM.Services.DataAccess.BusinessObjects.Fora;
using DM.Services.Forum.Dto.Output;
using Microsoft.EntityFrameworkCore;

namespace DM.Services.Forum.BusinessProcesses.Topics.Creating;

/// <inheritdoc />
internal class TopicCreatingRepository : ITopicCreatingRepository
{
    private readonly DmDbContext dbContext;
    private readonly IMapper mapper;

    /// <inheritdoc />
    public TopicCreatingRepository(
        DmDbContext dbContext,
        IMapper mapper)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }
        
    /// <inheritdoc />
    public async Task<Topic> Create(ForumTopic forumTopic)
    {
        dbContext.ForumTopics.Add(forumTopic);
        await dbContext.SaveChangesAsync();
        return await dbContext.ForumTopics
            .TagWith("DM.Forum.CreatedTopic")
            .Where(t => t.ForumTopicId == forumTopic.ForumTopicId)
            .ProjectTo<Topic>(mapper.ConfigurationProvider)
            .FirstAsync();
    }
}