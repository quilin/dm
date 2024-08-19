using DM.Services.Community.BusinessProcesses.Polls.Reading;
using DM.Services.Community.BusinessProcesses.Polls.Voting;
using DM.Services.DataAccess.MongoIntegration;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using DbPoll = DM.Services.DataAccess.BusinessObjects.Fora.Poll;

namespace DM.Services.Community.Storage.Storages.Polls;

/// <inheritdoc />
internal class PollVotingRepository(DmMongoClient client)
    : MongoCollectionRepository<DbPoll>(client), IPollVotingRepository
{
    /// <inheritdoc />
    public Task<Poll> Vote(Guid pollId, Guid optionId, Guid userId, CancellationToken cancellationToken) =>
        Collection.FindOneAndUpdateAsync(
            Filter.Eq(p => p.Id, pollId) &
            Filter.ElemMatch(p => p.Options, o => o.Id == optionId),
            Update.Push(u => u.Options.FirstMatchingElement().UserIds, userId),
            new FindOneAndUpdateOptions<DbPoll, Poll>
            {
                Projection = PollReadingRepository.PollProjection,
                ReturnDocument = ReturnDocument.After
            },
            cancellationToken);
}