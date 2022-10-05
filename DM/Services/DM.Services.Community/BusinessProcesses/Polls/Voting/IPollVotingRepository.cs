using System;
using System.Threading.Tasks;
using DM.Services.Community.BusinessProcesses.Polls.Reading;

namespace DM.Services.Community.BusinessProcesses.Polls.Voting;

/// <summary>
/// Storage for poll voting
/// </summary>
internal interface IPollVotingRepository
{
    /// <summary>
    /// Vote for the poll option
    /// </summary>
    /// <param name="pollId">Poll identifier</param>
    /// <param name="optionId">Option identifier</param>
    /// <param name="userId">User identifier</param>
    /// <returns></returns>
    Task<Poll> Vote(Guid pollId, Guid optionId, Guid userId);
}