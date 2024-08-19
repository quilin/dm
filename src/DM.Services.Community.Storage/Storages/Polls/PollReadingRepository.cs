using DM.Services.Community.BusinessProcesses.Polls.Reading;
using DM.Services.Core.Dto;
using DM.Services.DataAccess.MongoIntegration;
using MongoDB.Driver;
using DbPoll = DM.Services.DataAccess.BusinessObjects.Fora.Poll;

namespace DM.Services.Community.Storage.Storages.Polls;

/// <inheritdoc />
internal class PollReadingRepository(DmMongoClient client)
    : MongoCollectionRepository<DbPoll>(client), IPollReadingRepository
{
    /// <summary>
    /// Projection for poll
    /// </summary>
    public static readonly ProjectionDefinition<DbPoll, Poll> PollProjection = Project.Expression(p =>
        new Poll
        {
            Id = p.Id,
            StartDate = new DateTimeOffset(p.StartDate),
            EndDate = new DateTimeOffset(p.EndDate),
            Title = p.Title,
            Options = p.Options.Select(o => new PollOption
            {
                Id = o.Id,
                Text = o.Text,
                UserIds = o.UserIds
            })
        });

    /// <inheritdoc />
    public Task<long> Count(DateTimeOffset? activeUntil, CancellationToken cancellationToken) =>
        Collection.CountDocumentsAsync(activeUntil.HasValue
            ? Filter.Gte(p => p.EndDate, activeUntil.Value.UtcDateTime)
            : Filter.Empty,
            cancellationToken: cancellationToken);

    /// <inheritdoc />
    public async Task<IEnumerable<Poll>> Get(DateTimeOffset? activeUntil, PagingData pagingData,
        CancellationToken cancellationToken)
    {
        return await Collection
            .Find(activeUntil.HasValue
                ? Filter.Gte(p => p.EndDate, activeUntil.Value.UtcDateTime)
                : Filter.Empty)
            .Sort(Sort.Descending(p => p.StartDate))
            .Skip(pagingData.Skip)
            .Limit(pagingData.Take)
            .Project(PollProjection)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public Task<Poll> Get(Guid id, CancellationToken cancellationToken) => Collection
        .Find(Filter.Eq(p => p.Id, id))
        .Project(PollProjection)
        .FirstOrDefaultAsync(cancellationToken);
}