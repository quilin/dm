using System.Threading;
using System.Threading.Tasks;
using DM.Services.Community.BusinessProcesses.Polls.Reading;
using DM.Services.DataAccess.MongoIntegration;
using MongoDB.Driver;
using DbPoll = DM.Services.DataAccess.BusinessObjects.Fora.Poll;
using Poll = DM.Services.DataAccess.BusinessObjects.Fora.Poll;

namespace DM.Services.Community.BusinessProcesses.Polls.Creating;

/// <inheritdoc />
internal class PollCreatingRepository(DmMongoClient client)
    : MongoCollectionRepository<Poll>(client), IPollCreatingRepository
{
    /// <inheritdoc />
    public async Task<Reading.Poll> Create(DbPoll poll, CancellationToken cancellationToken)
    {
        await Collection.InsertOneAsync(poll, cancellationToken: cancellationToken);
        return await Collection.Find(Filter.Eq(p => p.Id, poll.Id))
            .Project(PollReadingRepository.PollProjection)
            .FirstAsync(cancellationToken);
    }
}