using System;
using System.Threading.Tasks;
using DM.Services.Community.BusinessProcesses.Polls.Reading;

namespace DM.Services.Community.BusinessProcesses.Polls.Voting
{
    /// <inheritdoc />
    public class PollVotingService : IPollVotingService
    {
        /// <inheritdoc />
        public Task<Poll> Vote(Guid optionId)
        {
            throw new NotImplementedException();
        }
    }
}