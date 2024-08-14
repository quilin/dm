using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DM.Services.Core.Dto;
using DM.Services.DataAccess.MongoIntegration;
using MongoDB.Driver;
using DbPoll = DM.Services.DataAccess.BusinessObjects.Fora.Poll;

namespace DM.Services.Community.BusinessProcesses.Polls.Reading;

/// <inheritdoc />
internal class PollReadingRepository : MongoCollectionRepository<DbPoll>, IPollReadingRepository
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
    public PollReadingRepository(DmMongoClient client) : base(client)
    {
    }

    /// <inheritdoc />
    public Task<long> Count(DateTimeOffset? activeUntil) =>
        Collection.CountDocumentsAsync(activeUntil.HasValue
            ? Filter.Gte(p => p.EndDate, activeUntil.Value.UtcDateTime)
            : Filter.Empty);

    /// <inheritdoc />
    public async Task<IEnumerable<Poll>> Get(DateTimeOffset? activeUntil, PagingData pagingData)
    {
        return await Collection
            .Find(activeUntil.HasValue
                ? Filter.Gte(p => p.EndDate, activeUntil.Value.UtcDateTime)
                : Filter.Empty)
            .Sort(Sort.Descending(p => p.StartDate))
            .Skip(pagingData.Skip)
            .Limit(pagingData.Take)
            .Project(PollProjection)
            .ToListAsync();
    }

    /// <inheritdoc />
    public Task<Poll> Get(Guid id) => Collection
        .Find(Filter.Eq(p => p.Id, id))
        .Project(PollProjection)
        .FirstOrDefaultAsync();
}