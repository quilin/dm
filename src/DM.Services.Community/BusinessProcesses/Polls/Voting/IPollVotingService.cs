using System;
using System.Threading.Tasks;
using DM.Services.Community.BusinessProcesses.Polls.Reading;

namespace DM.Services.Community.BusinessProcesses.Polls.Voting;

/// <summary>
/// Service for voting in poll
/// </summary>
public interface IPollVotingService
{
    /// <summary>
    /// Vote for a certain poll option
    /// </summary>
    /// <param name="pollId">Poll identifier</param>
    /// <param name="optionId">Option identifier</param>
    /// <returns></returns>
    Task<Poll> Vote(Guid pollId, Guid optionId);
}