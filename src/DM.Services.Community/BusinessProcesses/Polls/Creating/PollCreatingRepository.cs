using System.Threading.Tasks;
using DM.Services.Community.BusinessProcesses.Polls.Reading;
using DM.Services.DataAccess.MongoIntegration;
using MongoDB.Driver;
using Poll = DM.Services.DataAccess.BusinessObjects.Fora.Poll;

namespace DM.Services.Community.BusinessProcesses.Polls.Creating;

/// <inheritdoc />
internal class PollCreatingRepository : MongoCollectionRepository<Poll>, IPollCreatingRepository
{
    /// <inheritdoc />
    public PollCreatingRepository(DmMongoClient client) : base(client)
    {
    }

    /// <inheritdoc />
    public async Task<Reading.Poll> Create(Poll poll)
    {
        await Collection.InsertOneAsync(poll);
        return await Collection.Find(Filter.Eq(p => p.Id, poll.Id))
            .Project(PollReadingRepository.PollProjection)
            .FirstAsync();
    }
}