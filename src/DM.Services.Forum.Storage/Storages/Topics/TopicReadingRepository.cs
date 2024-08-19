using AutoMapper;
using AutoMapper.QueryableExtensions;
using DM.Services.Core.Dto;
using DM.Services.Core.Dto.Enums;
using DM.Services.Core.Extensions;
using DM.Services.DataAccess;
using DM.Services.Forum.BusinessProcesses.Topics.Reading;
using DM.Services.Forum.Dto.Output;
using Microsoft.EntityFrameworkCore;

namespace DM.Services.Forum.Storage.Storages.Topics;

/// <inheritdoc />
internal class TopicReadingRepository(
    DmDbContext dbContext,
    IMapper mapper) : ITopicReadingRepository
{
    private static readonly Guid NewsForumId = Guid.Parse("00000000-0000-0000-0000-000000000008");
    private static readonly Guid ErrorsForumId = Guid.Parse("00000000-0000-0000-0000-000000000006");

    /// <inheritdoc />
    public Task<int> Count(Guid forumId, CancellationToken cancellationToken) => dbContext.ForumTopics
        .TagWith("DM.Forum.TopicsCount")
        .CountAsync(t => !t.IsRemoved && t.ForumId == forumId && !t.Attached, cancellationToken);

    /// <inheritdoc />
    public async Task<IEnumerable<Topic>> Get(
        Guid forumId, PagingData pagingData, bool attached, CancellationToken cancellationToken)
    {
        var query = dbContext.ForumTopics
            .TagWith("DM.Forum.TopicsList")
            .Where(t => !t.IsRemoved && t.ForumId == forumId && t.Attached == attached)
            .ProjectTo<Topic>(mapper.ConfigurationProvider);

        IOrderedQueryable<Topic> orderedQuery;
        if (forumId == NewsForumId || attached)
        {
            orderedQuery = query.OrderByDescending(q => q.CreateDate);
        }
        else if (forumId == ErrorsForumId)
        {
            orderedQuery = query.OrderBy(q => q.Closed).ThenByDescending(q => q.LastActivityDate);
        }
        else
        {
            orderedQuery = query.OrderByDescending(q => q.LastActivityDate);
        }

        return await orderedQuery.Page(pagingData).ToArrayAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<Topic?> Get(Guid topicId, ForumAccessPolicy accessPolicy, CancellationToken cancellationToken) =>
        await dbContext.ForumTopics
            .TagWith("DM.Forum.Topic")
            .Where(t => !t.IsRemoved && t.ForumTopicId == topicId &&
                        (t.Forum.ViewPolicy & accessPolicy) != ForumAccessPolicy.NoOne)
            .ProjectTo<Topic>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);
}