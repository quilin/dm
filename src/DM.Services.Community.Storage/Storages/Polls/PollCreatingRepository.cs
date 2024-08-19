using DM.Services.Community.BusinessProcesses.Polls.Creating;
using DM.Services.Community.BusinessProcesses.Polls.Reading;
using DM.Services.DataAccess.MongoIntegration;
using MongoDB.Driver;
using DbPoll = DM.Services.DataAccess.BusinessObjects.Fora.Poll;

namespace DM.Services.Community.Storage.Storages.Polls;

/// <inheritdoc />
internal class PollCreatingRepository(DmMongoClient client)
    : MongoCollectionRepository<DbPoll>(client), IPollCreatingRepository
{
    /// <inheritdoc />
    public async Task<Poll> Create(DbPoll poll, CancellationToken cancellationToken)
    {
        await Collection.InsertOneAsync(poll, cancellationToken: cancellationToken);
        return await Collection.Find(Filter.Eq(p => p.Id, poll.Id))
            .Project(PollReadingRepository.PollProjection)
            .FirstAsync(cancellationToken);
    }
}