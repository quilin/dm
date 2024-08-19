using AutoMapper;
using AutoMapper.QueryableExtensions;
using DM.Services.DataAccess;
using DM.Services.DataAccess.BusinessObjects.Fora;
using DM.Services.Forum.BusinessProcesses.Topics.Creating;
using DM.Services.Forum.Dto.Output;
using Microsoft.EntityFrameworkCore;

namespace DM.Services.Forum.Storage.Storages.Topics;

/// <inheritdoc />
internal class TopicCreatingRepository(
    DmDbContext dbContext,
    IMapper mapper) : ITopicCreatingRepository
{
    /// <inheritdoc />
    public async Task<Topic> Create(ForumTopic forumTopic, CancellationToken cancellationToken)
    {
        dbContext.ForumTopics.Add(forumTopic);
        await dbContext.SaveChangesAsync(cancellationToken);
        return await dbContext.ForumTopics
            .TagWith("DM.Forum.CreatedTopic")
            .Where(t => t.ForumTopicId == forumTopic.ForumTopicId)
            .ProjectTo<Topic>(mapper.ConfigurationProvider)
            .FirstAsync(cancellationToken);
    }
}