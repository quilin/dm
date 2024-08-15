using System;
using System.Threading.Tasks;
using DM.Services.Community.BusinessProcesses.Polls.Reading;
using DM.Services.DataAccess.MongoIntegration;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using DbPoll = DM.Services.DataAccess.BusinessObjects.Fora.Poll;

namespace DM.Services.Community.BusinessProcesses.Polls.Voting;

/// <inheritdoc />
internal class PollVotingRepository : MongoCollectionRepository<DbPoll>, IPollVotingRepository
{
    /// <inheritdoc />
    public PollVotingRepository(DmMongoClient client) : base(client)
    {
    }

    /// <inheritdoc />
    public Task<Poll> Vote(Guid pollId, Guid optionId, Guid userId) =>
        Collection.FindOneAndUpdateAsync(
            Filter.Eq(p => p.Id, pollId) &
            Filter.ElemMatch(p => p.Options, o => o.Id == optionId),
            Update.Push(u => u.Options.FirstMatchingElement().UserIds, userId),
            new FindOneAndUpdateOptions<DbPoll, Poll>
            {
                Projection = PollReadingRepository.PollProjection,
                ReturnDocument = ReturnDocument.After
            });
}