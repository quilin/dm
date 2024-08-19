using AutoMapper;
using AutoMapper.QueryableExtensions;
using DM.Services.DataAccess;
using DM.Services.DataAccess.BusinessObjects.Fora;
using DM.Services.DataAccess.RelationalStorage;
using DM.Services.Forum.BusinessProcesses.Topics.Updating;
using DM.Services.Forum.Dto.Output;
using Microsoft.EntityFrameworkCore;

namespace DM.Services.Forum.Storage.Storages.Topics;

/// <inheritdoc />
internal class TopicUpdatingRepository(
    DmDbContext dbContext,
    IMapper mapper) : ITopicUpdatingRepository
{
    /// <inheritdoc />
    public async Task<Topic> Update(IUpdateBuilder<ForumTopic> updateBuilder, CancellationToken cancellationToken)
    {
        var topicId = updateBuilder.AttachTo(dbContext);
        await dbContext.SaveChangesAsync(cancellationToken);
        return await dbContext.ForumTopics
            .TagWith("DM.Forum.UpdatedTopic")
            .Where(t => t.ForumTopicId == topicId)
            .ProjectTo<Topic>(mapper.ConfigurationProvider)
            .FirstAsync(cancellationToken);
    }
}